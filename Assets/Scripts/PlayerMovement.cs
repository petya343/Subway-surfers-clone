using System;
using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using static UnityEngine.GraphicsBuffer;
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;
    private BoxCollider col;
    private bool isGrounded = true;

    private float jumpForce = 7f;
    private float forwardSpeed = 7f;
    private float smoothSpeed = 15f;
    private float sideMove = 4f;
    private float x;
    private Vector3 colSize;
    private Vector3 colCenter;
    private int line = 0;
    private bool isDead = false;
    private bool boots = false;
    private float bootsJumpForce = 10f;
    private float bootsDuration = 10f;
    private Coroutine bootsCoroutine = null;

    [SerializeField]
    private ParticleSystem jumpEffectLeft;
    [SerializeField]
    private ParticleSystem jumpEffectRight;
    [SerializeField]
    private GameObject leftboot;
    [SerializeField]
    private GameObject rightboot;
    [SerializeField]
    private ParticleSystem collectingCoins;
    [SerializeField]
    private CoinsUI coinsUI;
    [SerializeField]
    private ScoreUI scoreUI;
    [SerializeField]
    private GameObject bootsUI;
    private int posUI = 0;
    private Vector3 bootsUIpos;

    private float scoreStartPoint;
    private int score = 0;
    private int coinsCounter = 0;
    private bool OnTrain = false;
    private bool gameStarted = false;
    private bool musicIndicator = true;
    [SerializeField]
    private GameObject endScreen;

    private Camera cam;
    private ScreenShaker screenShaker;

    [SerializeField]
    private AudioSource coinCollect;
    [SerializeField]
    private AudioSource powerCollect;
    [SerializeField]
    private AudioSource bootsJump;
    [SerializeField]
    private AudioSource hit;
    [SerializeField]
    private AudioSource gameOver;
    [SerializeField]
    private AudioSource backgroundMusic;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();
        colSize = col.size;
        colCenter = col.center;
        animator = GetComponent<Animator>();
        x = transform.position.x;
        scoreStartPoint = transform.position.z;
        bootsUIpos = bootsUI.transform.position;
        cam = Camera.main;
        screenShaker = cam.GetComponent<ScreenShaker>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!gameStarted) return;
        if (musicIndicator)
        {
            backgroundMusic.Play();
            musicIndicator = false;
        }
        forwardSpeed += 0.0001f;
        score = Mathf.RoundToInt(transform.position.z - scoreStartPoint);
        scoreUI.UpdateScore(score);

        if (Input.GetKeyDown(KeyCode.A) && line != -1 && !isDead)
        {
            x -= sideMove;
            --line;
        }

        if (Input.GetKeyDown(KeyCode.D) && line != 1 && !isDead)
        {
            x += sideMove;
            ++line;
        }

        if (!isDead)
        {
            transform.position = new Vector3(Mathf.MoveTowards(transform.position.x, x, smoothSpeed * Time.deltaTime),
                                             transform.position.y,
                                             transform.position.z + forwardSpeed * Time.deltaTime);

        }

        if (Input.GetAxisRaw("Vertical") == 1 && isGrounded && !isDead)
        {
            if (!boots)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            }
            else
            {
                bootsJump.Play();
                rb.velocity = new Vector3(rb.velocity.x, bootsJumpForce, rb.velocity.z);
                jumpEffectLeft.Play();
                jumpEffectRight.Play();
            }
            isGrounded = false;
            animator.SetBool("isJumping", true);
        }
        if (Input.GetKeyDown(KeyCode.S) && !isDead)
        {
            if (!isGrounded)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                rb.AddForce(Vector3.up * -10f, ForceMode.VelocityChange);
            }
                animator.SetTrigger("Sliding");
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            OnTrain = false;
            animator.SetBool("isJumping", false);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("TrainSurface"))
        {
            isGrounded = true;
            OnTrain = true;
            animator.SetBool("isJumping", false);
        }
        if (other.gameObject.CompareTag("Obstacle"))
        {
            hit.Play();
            Die();
            GetComponent<ExtraLife>().ReviveCharacter();
            if (!GetComponent<ExtraLife>().HasExtraLife())
            {
                backgroundMusic.Stop();
                gameOver.Play();
                endScreen.gameObject.SetActive(true);
                StartCoroutine(EndGame());
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("TrainSurface"))
        {
            isGrounded = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            collectingCoins.Play();
            coinCollect.Play();
            Destroy(other.gameObject);
            coinsCounter++;
            coinsUI.UpdateCoins(coinsCounter);
        }
        if (other.gameObject.name.Contains("Boots"))
        {
            //boots = true;
            powerCollect.Play();
            if (GetComponent<MagnetPower>().PosUI() == 1 && !boots) //&& !boots
            {
                bootsUI.transform.position += new Vector3(160f, 0f, 0f);
                posUI = 2;
            }
            else if (!boots)
            {
                posUI = 1;
            }
            if (bootsCoroutine != null)
            {
                StopCoroutine(bootsCoroutine);
            }
            bootsUI.GetComponent<BarUI>().ResetTimer();
            bootsUI.SetActive(true);
            bootsCoroutine = StartCoroutine(ActivateBoots());
            Destroy(other.gameObject);
        }
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

    private IEnumerator ActivateBoots()
    {
        boots = true;
        leftboot.SetActive(true);
        rightboot.SetActive(true);
        yield return new WaitForSeconds(bootsDuration);
        boots = false;
        posUI = 0;
        bootsUI.SetActive(false);
        leftboot.SetActive(false);
        rightboot.SetActive(false);
        bootsUI.transform.position = bootsUIpos;
        bootsCoroutine = null;
    }

    public bool Boots() => boots;
    public bool isOnTrain() => OnTrain;
    public bool Dead() => isDead;
    public void setDead(bool value) => isDead = value;
    public void Die()
    {
        isDead = true;
        isGrounded = true;
        transform.position += new Vector3(0f, 0f, -0.5f);
        animator.SetBool("isDead", true);
        screenShaker.ScreenShake();
    }
    public int Line() => line;
    public void ResetLine()
    {
        line = 0;
        x = 3.2f;
    }
    public int PosUI() => posUI;

    public void SetGame(bool value) => gameStarted = value;

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(0);
    }

}
