using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ExtraLife : MonoBehaviour
{
    public IGameManager GameManagerService {  get; set; }
    public  IObstaclesGenerating ObstaclesGeneratingService { get; set; }
    public IPlayerController PlayerControllerService { get; set; }

    private bool heart = false;
    private GameObject obstaclesGenerator;
    private Animator animator;
    private Rigidbody rb;
    [SerializeField]
    private Image heartUI;
    [SerializeField]
    private Sprite fullHeart;
    [SerializeField]
    private Sprite emptyHeart;
    [SerializeField]
    private GameObject usingHeartUI;

    void Start()
    {
        obstaclesGenerator = GameObject.Find("ObstaclesGenerator");
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        heartUI.sprite = emptyHeart;
        if (PlayerControllerService == null) PlayerControllerService = PlayerController.Instance;
        if (GameManagerService == null) GameManagerService = GameManager.Instance;
        if (ObstaclesGeneratingService == null) ObstaclesGeneratingService = ObstaclesGenerating.Instance;
    }
    public void ReviveCharacter()
    {
        if (PlayerControllerService.IsDead && heart)
        {
            StartCoroutine(HeartCoroutine());
            heartUI.sprite = emptyHeart;
        }
    }

    public void ActivateHeart()
    {
        if (!heart) heart = true;
        heartUI.sprite = fullHeart;
    }
    private IEnumerator HeartCoroutine()
    {
        yield return new WaitForSeconds(3f);

       GameManagerService.IsGameRunning = false;

        heart = false;
        if (animator != null)
        {
            animator.SetBool("isDead", false);
            animator.SetBool("isJumping", false);
            animator.ResetTrigger("Sliding");
            animator.SetBool("Idle", true);
        }

        PlayerControllerService.ResetLine();
        PlayerControllerService.IsDead = false;

        transform.position = ObstaclesGeneratingService.getLastPosition();

        usingHeartUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        usingHeartUI.gameObject.SetActive(false);

        GameManagerService.IsGameRunning = true;
        if (animator != null) animator.SetBool("Idle", false);
    }

    public bool HasExtraLife() => heart;
    public void SetHeart(bool value) => heart = value;
    public Image HeartUI() => heartUI;
    public void SetHeartUI(Image img) => heartUI = img;
    public GameObject UsingHeartUI() => usingHeartUI;
    public void SetHeartUIObject(GameObject obj) => usingHeartUI = obj;


}
