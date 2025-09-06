using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour, IPlayerController
{
    public static PlayerController Instance;

    private Rigidbody rb;
    private Animator animator;
    private BoxCollider col;
    private float x;
    private Vector3 colSize;
    private Vector3 colCenter;

    private float jumpForce = 7f;
    private float forwardSpeed = 7f;
    private float smoothSpeed = 15f;
    private float sideMove = 4f;

    public IGameManager GameManagerService { get; set; }
    public IScoreSystem ScoreSystemService { get; set; }
    public IAudioManager AudioManagerService { get; set; }
    public IBoots BootsService { get; set; }

    public int line { get; private set; } = 0;
    public bool IsDead { get; set; }
    public bool IsOnTrain { get; set; }

    public bool isGrounded { get; set; } = true;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();
        colSize = col.size;
        colCenter = col.center;
        animator = GetComponent<Animator>();
        x = transform.position.x;
        if (GameManagerService == null) GameManagerService = GameManager.Instance;
        if (ScoreSystemService == null) ScoreSystemService = ScoreSystem.Instance;
        if (AudioManagerService == null) AudioManagerService = AudioManager.Instance;
        if (BootsService == null) BootsService = Boots.Instance;
    }
    void Update()
    {
        if (IsDead || GameManagerService == null || !GameManagerService.IsGameRunning) return;

        forwardSpeed += 0.0003f;
        HandleInput();
        MoveForward();

        ScoreSystemService?.UpdateScore();

    }

    private void HandleInput()
    {
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && line != -1)
        {
            x -= sideMove;
            --line;
        }

        if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && line != 1)
        {
            x += sideMove;
            ++line;
        }

        if (Input.GetAxisRaw("Vertical") == 1 && isGrounded)
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            Slide();
        }

    }

    public void Jump()
    {
        float finalJumpForce = (BootsService != null && BootsService.BootsActive) ?
                          BootsService.BootsJumpForce :
                          jumpForce;

        rb.velocity = new Vector3(rb.velocity.x, finalJumpForce, rb.velocity.z);
        isGrounded = false;
        if (animator != null) animator.SetBool("isJumping", true);

        if (BootsService.BootsActive)
        {
            BootsService.PlayBootsJumpEffects();
        }
    }

    public void MoveForward()
    {
        transform.position = new Vector3(Mathf.MoveTowards(transform.position.x, x, smoothSpeed * Time.deltaTime),
                                         transform.position.y,
                                         transform.position.z + forwardSpeed * Time.deltaTime);

    }

    public void Slide()
    {
        if (!isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * -10f, ForceMode.VelocityChange);
        }
        animator.SetTrigger("Sliding");
    }

    public void Die()
    {
        IsDead = true;
        //isGrounded = true;
        transform.position += new Vector3(0f, 0f, -1f);
        if (animator != null) animator.SetBool("isDead", true);
        if (Camera.main != null) Camera.main.GetComponent<ScreenShaker>().ScreenShake();
        GetComponent<ExtraLife>()?.ReviveCharacter();
    }
    public void ResetLine()
    {
        line = 0;
        x = 3.2f;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isJumping", false);
            IsOnTrain = false;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("TrainSurface"))
        {
            isGrounded = true;
            animator.SetBool("isJumping", false);
            IsOnTrain = true;
        }

        if (other.gameObject.CompareTag("Obstacle"))
        {
            AudioManager.Instance.PlayHit();
            Die();
            GetComponent<ExtraLife>().ReviveCharacter();
            if (!GetComponent<ExtraLife>().HasExtraLife())
            {
                GameManager.Instance.GameOver();
            }
        }
    }
    public void EndSliding()
    {
        if (col == null) return;
        col.size = colSize;
        col.center = colCenter;
    }
    public void StartSliding()
    {
        if (col == null) return;
        col.size = new Vector3(col.size.x, 1.5f, col.size.z);
        col.center = new Vector3(col.center.x, 0.8f, col.center.z);
    }
}
