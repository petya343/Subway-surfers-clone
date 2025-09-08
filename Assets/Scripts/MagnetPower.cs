using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetPower : MonoBehaviour, IMagnetPower
{
    public static MagnetPower Instance { get; private set; }
    public IBoots BootsService { get; set; } 
    public IBarUI BarUIService { get; set; }

    private float magnetRange = 12f;
    private float duration = 10f;
    private float magnetSpeed = 15f;

    private bool isMagnetActive = false;
    private Coroutine magnetCoroutine = null;
    public GameObject magnetUI;
    private int posUI = 0;
    private Vector3 magnetUIpos;
    [SerializeField]
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        magnetUIpos = magnetUI.transform.position;
        if (BootsService == null) BootsService = GetComponent<Boots>();
        if (BarUIService == null) BarUIService = magnetUI.GetComponent<BarUI>();
    }

    public bool MagnetActive() => isMagnetActive;
    void Update()
    {
        if (isMagnetActive)
        {
            GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");
            foreach (GameObject coin in coins)
            {
                float distance = Vector3.Distance(transform.position, coin.transform.position);
                if (distance <= magnetRange && coin.transform.position.z >= transform.position.z)
                {
                    coin.transform.position = Vector3.MoveTowards(coin.transform.position,
                                                                  transform.position,
                                                                  magnetSpeed * Time.deltaTime);

                    PulledCoin co = coin.GetComponent<PulledCoin>();
                    co.StartPull();
                }
            }
        }
    }

    public void ActivateMagnet()
    {
        if (BootsService != null && BootsService.PosUI() == 1 && !isMagnetActive)
        {
            magnetUI.transform.position += new Vector3(160f, 0f, 0f);
            posUI = 2;
        }
        else if (!isMagnetActive)
        {
            posUI = 1;
        }
        if (magnetCoroutine != null)
        {
            StopCoroutine(magnetCoroutine);
            if (BarUIService != null) BarUIService.ResetTimer();
            else magnetUI.GetComponent<BarUI>()?.ResetTimer();
        }
        magnetUI.SetActive(true);
        magnetCoroutine = StartCoroutine(MagnetCoroutine());

    }

    private IEnumerator MagnetCoroutine()
    {
        isMagnetActive = true;
        yield return new WaitForSeconds(duration);
        isMagnetActive = false;
        posUI = 0;
        magnetUI.SetActive(false);
        magnetUI.transform.position = magnetUIpos;
        magnetCoroutine = null;
    }

    public int PosUI() => posUI;

}
