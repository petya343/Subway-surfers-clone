using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartCountDown : MonoBehaviour
{
    [SerializeField]
    private TMP_Text countDownText;
    private float currentTime = 3f;
    private GameObject player;
    private Animator animator;
    
    void Start()
    {
        player = GameObject.Find("Player");
        animator = player.GetComponent<Animator>();
        animator.SetBool("Idle", true);
        StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
   {
        float passed = 0f;
        float duration = 0.5f;
        AudioManager.Instance.PlayStartCounter();
        while (currentTime > 0f)
        {
            countDownText.text = currentTime.ToString();
            passed = 0f;
            duration = 0.5f;
            while (passed < duration)
            {
                passed += Time.deltaTime;
                countDownText.fontSize = Mathf.Lerp(countDownText.fontSize, 180f, 15f * Time.deltaTime);
                yield return null;
            }

            countDownText.fontSize = 180f;
            yield return new WaitForSeconds(0.5f);
            currentTime--;
            countDownText.fontSize = 70f;

        }

        countDownText.text = "GO!";
        passed = 0f;
        duration = 0.5f;
        while (passed < duration)
        {
            passed += Time.deltaTime;
            countDownText.fontSize = Mathf.Lerp(countDownText.fontSize, 180f, 15f * Time.deltaTime);
            yield return null;
        }

        countDownText.fontSize = 180f;
        yield return new WaitForSeconds(0.5f);

        countDownText.gameObject.SetActive(false);
        animator.SetBool("Idle", false);
        GameManager.Instance.StartGame();
    }

}
