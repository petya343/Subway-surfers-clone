using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject pause;
    
    public void EndGame()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }

    public void Settings()
    {
        SceneManager.LoadScene(2);
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        pause.SetActive(true);
        Time.timeScale = 1.0f;
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        pause.SetActive(false);
        Time.timeScale = 0f;
    }
  
}
