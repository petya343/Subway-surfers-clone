using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject settingsMenu;
    [SerializeField]
    private GameObject pause;

    public static GameSceneMenu Instance { get; private set; }

    public bool GameIsPaused { get; private set; } = false;
    public void Settings()
    {
        settingsMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }
    public void EndGame()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }

    public void ResumeGame()
    {
        GameIsPaused = false;
        pauseMenu.SetActive(false);
        pause.SetActive(true);
        AudioManager.Instance.UnPauseBackgroundMusic();
        Time.timeScale = 1.0f;
    }

    public void PauseGame()
    {
        GameIsPaused = true;
        pauseMenu.SetActive(true);
        pause.SetActive(false);
        AudioManager.Instance.PauseBackgroundMusic();
        Time.timeScale = 0f;
    }

}
