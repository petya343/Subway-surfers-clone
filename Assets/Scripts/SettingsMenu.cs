using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject pauseMenu;

    private void Start()
    {

        float music = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sound = PlayerPrefs.GetFloat("SoundVolume", 1f);

        musicSlider.value = music;
        soundSlider.value = sound;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMusicVolume(music);
            AudioManager.Instance.SetSoundVolume(sound);
        }

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        soundSlider.onValueChanged.AddListener(SetSoundVolume);

    }
    private void SetMusicVolume(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();

        if (AudioManager.Instance != null)
            AudioManager.Instance.SetMusicVolume(value);
    }

    private void SetSoundVolume(float value)
    {
        PlayerPrefs.SetFloat("SoundVolume", value);
        PlayerPrefs.Save();

        if (AudioManager.Instance != null)
            AudioManager.Instance.SetSoundVolume(value);

    }
    public void Back()
    {
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }
    
}
