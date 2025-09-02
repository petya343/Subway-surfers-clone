using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }
    public void Back()
    {
        SceneManager.LoadScene(0);
    }
    
}
