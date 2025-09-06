using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boots : MonoBehaviour, IBoots
{
    public static Boots Instance { get; private set; }
    public IBarUI BarUIService { get; set; }
    public IAudioManager AudioManagerService { get; set; }
    public IMagnetPower MagnetService { get; set; }

    public ParticleSystem jumpEffectLeft;
    public ParticleSystem jumpEffectRight;
    public GameObject leftBoot;
    public GameObject rightBoot;
    public GameObject bootsUI;

    private float bootsJumpForce = 10f;
    private float bootsDuration = 10f;
    private Vector3 bootsUIpos;
    private bool bootsActive;
    private Coroutine bootsCoroutine;
    private int posUI = 0;
    public bool BootsActive => bootsActive;
    public float BootsJumpForce => bootsJumpForce;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        bootsUIpos = bootsUI.transform.position;
        if (AudioManagerService == null) AudioManagerService = AudioManager.Instance;
        if (MagnetService == null) MagnetService = MagnetPower.Instance;
        if (BarUIService == null) BarUIService = BarUI.Instance;
    }

    public void ActivateBoots()
    {
        AudioManagerService?.PlayPowerCollect();

        if (MagnetService != null && MagnetService.PosUI() == 1 && !BootsActive)
        {
            bootsUI.transform.position += new Vector3(160f, 0f, 0f);
            posUI = 2;
        }
        else if (!BootsActive)
        {
            posUI = 1;
        }
        if (bootsCoroutine != null)
        {
            StopCoroutine(bootsCoroutine);
            if (BarUIService != null) BarUIService.ResetTimer();
            else bootsUI.GetComponent<BarUI>()?.ResetTimer();
        }
        bootsUI.SetActive(true);
        bootsCoroutine = StartCoroutine(BootsCoroutine());
    }

    private IEnumerator BootsCoroutine()
    {
        bootsActive = true;
        leftBoot.SetActive(true);
        rightBoot.SetActive(true);

        yield return new WaitForSeconds(bootsDuration);

        bootsActive = false;
        leftBoot.SetActive(false);
        rightBoot.SetActive(false);
        bootsUI.SetActive(false);
        posUI = 0;
        bootsUI.transform.position = bootsUIpos; 
        bootsCoroutine = null;
    }

    public void PlayBootsJumpEffects()
    {
        jumpEffectLeft.Play();
        jumpEffectRight.Play();
    }

    public int PosUI() => posUI;
}
