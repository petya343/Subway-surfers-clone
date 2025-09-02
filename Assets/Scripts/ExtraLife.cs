using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ExtraLife : MonoBehaviour
{
    private bool heart = false;
    private GameObject obstaclesGenerator;
    private GameObject housesGenerator;
    private GameObject railsGenerator;
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
        //housesGenerator = GameObject.Find("HousesGenerator");
        //railsGenerator = GameObject.Find("RailsGenerator");
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        heartUI.sprite = emptyHeart;
    }
    public void ReviveCharacter()
    {
        if (gameObject.GetComponent<PlayerMovement>().Dead() && heart)
        {
            //GetComponent<PlayerMovement>().enabled = false;
            StartCoroutine(ActivateHeart());
            heartUI.sprite = emptyHeart;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Heart"))
        {
            if(!heart) heart = true;
            heartUI.sprite = fullHeart;
            Destroy(other.gameObject);
        }
    }
    private IEnumerator ActivateHeart()
    {
        yield return new WaitForSeconds(3f);

        GetComponent<PlayerMovement>().SetGame(false);
        //GetComponent<PlayerMovement>().enabled = true;

        heart = false;
        animator.SetBool("isDead", false);
        animator.SetBool("isJumping", false);
        animator.ResetTrigger("Sliding");
        animator.SetBool("Idle", true);

        gameObject.GetComponent<PlayerMovement>().ResetLine();
        gameObject.GetComponent<PlayerMovement>().setDead(false);

        transform.position = obstaclesGenerator.GetComponent<ObstaclesGenerating>().getLastPosition();
        

        usingHeartUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        usingHeartUI.gameObject.SetActive(false);

        GetComponent<PlayerMovement>().SetGame(true);
        animator.SetBool("Idle", false);


    }

    public bool HasExtraLife() => heart;

}
//obstaclesGenerator.GetComponent<ObstaclesGenerating>().Pause();
        //housesGenerator.GetComponent<HouseGenerating>().Pause();
        //railsGenerator.GetComponent<RailsGenerating>().Pause();
//rb.isKinematic = false;
//rb.velocity = Vector3.zero;

//obstaclesGenerator.GetComponent<ObstaclesGenerating>().Resume();
//housesGenerator.GetComponent<HouseGenerating>().Resume();
//railsGenerator.GetComponent<RailsGenerating>().Resume();