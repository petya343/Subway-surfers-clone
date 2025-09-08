using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem collectingCoins;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            AudioManager.Instance.PlayCoinCollect();
            collectingCoins.Play();
            GameManager.Instance.AddCoin(1);
            Destroy(other.gameObject);
        }

        if (other.gameObject.name.Contains("Boots"))
        {
            AudioManager.Instance.PlayPowerCollect();
            GetComponent<Boots>().ActivateBoots();
            Destroy(other.gameObject);
        }

        if (other.gameObject.name.Contains("Magnet"))
        {
            AudioManager.Instance.PlayPowerCollect();
            GetComponent<MagnetPower>().ActivateMagnet();
            Destroy(other.gameObject);
        }

        if (other.gameObject.name.Contains("Heart"))
        {
            AudioManager.Instance.PlayPowerCollect();
            GetComponent<ExtraLife>().ActivateHeart();
            Destroy(other.gameObject);
        }
    }
}
