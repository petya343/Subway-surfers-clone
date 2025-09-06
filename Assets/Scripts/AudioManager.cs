using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour, IAudioManager
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource coinCollect, powerCollect, bootsJump, hit, gameOver, backgroundMusic, startCounter;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;
    private AudioSource[] musicSources;

    public float MusicVolume { get; private set; } = 0.5f;
    public float SoundVolume { get; private set; } = 0.5f;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        musicSources = new AudioSource[] { coinCollect, powerCollect, bootsJump, hit, gameOver, startCounter };
    }

    private void Start()
    {
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        soundSlider.onValueChanged.AddListener(SetSoundVolume);
    }

    public void PlayCoinCollect() => coinCollect.Play();
    public void PlayPowerCollect() => powerCollect.Play();
    public void PlayBootsJump() => bootsJump.Play();
    public void PlayHit() => hit.Play();
    public void PlayGameOver() => gameOver.Play();
    public void PlayStartCounter() => startCounter.Play();
    public void PauseStartCounter() => startCounter.Pause();
    public void UnPauseStartCounter() => startCounter.UnPause();
    public void PlayBackgroundMusic() => backgroundMusic.Play();
    public void StopBackgroundMusic() => backgroundMusic.Stop();
    public void PauseBackgroundMusic() => backgroundMusic.Pause();
    public void UnPauseBackgroundMusic() => backgroundMusic.UnPause();

    public void SetMusicVolume(float volume)
    {
        MusicVolume = volume;
        backgroundMusic.volume = volume;
    }

    public void SetSoundVolume(float volume)
    {
        SoundVolume = volume;
        foreach (var source in musicSources)
        {
            source.volume = volume;
        }
    }
}
