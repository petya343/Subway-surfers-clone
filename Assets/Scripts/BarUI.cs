using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarUI : MonoBehaviour, IBarUI
{
    public static BarUI Instance { get; private set; }

    [SerializeField]
    private Image barImage;
    private float duration = 10f;
    private float timeLeft;
    public Image BarImage
    {
        get => barImage;
        set => barImage = value;
    }

    private void Awake()
    {
        Instance = this;
    }
    void OnEnable()
    {
        ResetTimer();
    }

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
