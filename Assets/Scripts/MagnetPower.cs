using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetPower : MonoBehaviour
{
    private float magnetRange = 12f;
    private float duration = 10f;
    private float magnetSpeed = 15f;
    
    private bool isMagnetActive = false;
    private Coroutine magnetCoroutine = null;
    [SerializeField]
    private ParticleSystem collectingCoins;
    [SerializeField]
    private GameObject magnetUI;
    private int posUI = 0;
    private Vector3 magnetUIpos;
    [SerializeField]
    private AudioSource powerCollect;
    private void Start()
    {
        magnetUIpos = magnetUI.transform.position;
    }

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
                //if (distance < 1f)
                //{
                //    CollectCoin(coin);
                //}
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Magnet"))
        {
            powerCollect.Play();
            if (GetComponent<PlayerMovement>().PosUI() == 1 && !isMagnetActive)
            {
                magnetUI.transform.position += new Vector3(160f, 0f, 0f);
                posUI = 2;
            }
            else if(!isMagnetActive)
            {
                posUI = 1;
            }
            if (magnetCoroutine != null)
            {
                StopCoroutine(magnetCoroutine);
                magnetUI.GetComponent<BarUI>().ResetTimer();
            }
            magnetUI.SetActive(true);
            magnetCoroutine = StartCoroutine(ActivateMagnet());
            Destroy(other.gameObject);
        }
    }

    private IEnumerator ActivateMagnet()
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

    //public void CollectCoin(GameObject coin)
    //{
    //    collectingCoins.Play();
    //    Destroy(coin);
    //}
}
