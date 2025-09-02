using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarUI : MonoBehaviour
{
    [SerializeField]
    private Image barImage;
    private float duration = 10f;
    private float timeLeft;

    void OnEnable()
    {
        ResetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            barImage.fillAmount = timeLeft / duration;
        }
    }

    public void ResetTimer()
    {
        timeLeft = duration;
        barImage.fillAmount = 1f;
    }
}
