using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;
    private BoxCollider col;
    private bool isGrounded = true;
    private float x;
    private Vector3 colSize;
    private Vector3 colCenter;
    private int line = 0;
    private bool isDead = false;

    private float jumpForce = 7f;
    private float forwardSpeed = 7f;
    private float smoothSpeed = 15f;
    private float sideMove = 4f;
    public bool IsDead { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();
        colSize = col.size;
        colCenter = col.center;
        animator = GetComponent<Animator>();
        x = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDead || !GameManager.Instance.IsGameRunning) return;

        HandleInput();
        MoveForward();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.A) && line != -1)
        {
            x -= sideMove;
            --line;
        }

        if (Input.GetKeyDown(KeyCode.D) && line != 1)
        {
            x += sideMove;
            ++line;
        }

        if (Input.GetAxisRaw("Vertical") == 1 && isGrounded)
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Slide();
        }

    }

    private void Jump()
    {
        float finalJumpForce = Boots.Instance.BootsActive ?
                          Boots.Instance.BootsJumpForce :
                          jumpForce;

        rb.velocity = new Vector3(rb.velocity.x, finalJumpForce, rb.velocity.z);
        isGrounded = false;
        animator.SetBool("isJumping", true);

        if (Boots.Instance.BootsActive)
        {
            Boots.Instance.PlayBootsJumpEffects();
        }
    }

    private void MoveForward()
    {
        transform.position = new Vector3(Mathf.MoveTowards(transform.position.x, x, smoothSpeed * Time.deltaTime),
                                             transform.position.y,
                                             transform.position.z + forwardSpeed * Time.deltaTime);

    }

    private void Slide()
    {
        if (!isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * -10f, ForceMode.VelocityChange);
        }
        animator.SetTrigger("Sliding");
    }

    public void EndSliding()
    {
        col.size = colSize;
        col.center = colCenter;
    }
    public void StartSliding()
    {
        col.size = new Vector3(col.size.x, 1.5f, col.size.z);
        col.center = new Vector3(col.center.x, 0.8f, col.center.z);
    }

    public void Die()
    {
        IsDead = true;
        //isGrounded = true;
        transform.position += new Vector3(0f, 0f, -1f);
        animator.SetBool("isDead", true);
        Camera.main.GetComponent<ScreenShaker>().ScreenShake();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground") || other.gameObject.layer == LayerMask.NameToLayer("TrainSurface"))
        {
            isGrounded = true;
            animator.SetBool("isJumping", false);
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
}
