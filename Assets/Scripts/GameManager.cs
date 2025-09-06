using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IGameManager
{
    public static GameManager Instance { get; set; }

    public bool IsGameRunning { get; set; }

    private int coins = 0;
    private GameObject player;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        if (IsGameRunning && !player.GetComponent<PlayerController>().IsDead)
        {
            player.GetComponent<ScoreSystem>().UpdateScore();
        }
    }
    public void StartGame()
    {
        coins = 0;
        IsGameRunning = true;
        AudioManager.Instance.PlayBackgroundMusic();
    }
    public void GameOver()
    {
        IsGameRunning = false;
        AudioManager.Instance.StopBackgroundMusic();
        AudioManager.Instance.PlayGameOver();
        UIManager.Instance.ShowGameOverScreen();
        StartCoroutine(BackToMenu());
    }

    public void AddCoin(int amount)
    {
        coins += amount;
        UIManager.Instance.UpdateCoins(coins);
    }

    private IEnumerator BackToMenu()
    {
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(0);
    }
}
