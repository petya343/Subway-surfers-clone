using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField]
    private TMP_Text scoreText;
    [SerializeField]
    private TMP_Text coinsText;
    [SerializeField] 
    private GameObject endScreen;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateScore(int score) => scoreText.text = score.ToString();
    public void UpdateCoins(int coins) => coinsText.text = coins.ToString();
    public void ShowGameOverScreen() => endScreen.SetActive(true);

}
