using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulledCoin : MonoBehaviour
{
    private GameObject player;
    private float speed = 30f;
    private bool isPulled = false;

    public void StartPull()
    {
        player = GameObject.Find("Player");
        isPulled = true;
    }

    void Update()
    {
        if (isPulled && player != null)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                                                     player.transform.position,
                                                     speed * Time.deltaTime);

            float distance = Vector3.Distance(transform.position, player.transform.position);

            //if (distance < 1f)
            //{
            //    player.GetComponent<MagnetPower>().CollectCoin(gameObject);
            //}
        }
    }
}
