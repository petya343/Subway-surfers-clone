using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameObject player;
    private float offsetZ = -7f;
    private float offsetY;
    private float offsetX;
    private Vector3 cameraPos;
    private float fixedY;
    private float fixedTrainY;
    private bool isJumpingWithBoots = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");   
        fixedY = transform.position.y;
        fixedTrainY = fixedY;
        cameraPos = transform.position;
        offsetY = transform.position.y - player.transform.position.y;
        offsetX = transform.position.x - player.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.GetComponent<PlayerController>().IsOnTrain && !player.GetComponent<PlayerController>().isGrounded && player.GetComponent<Boots>().BootsActive) isJumpingWithBoots = true;
        else if (player.GetComponent<PlayerController>().isGrounded && !player.GetComponent<Boots>().BootsActive) isJumpingWithBoots = false;

        if (player.GetComponent<PlayerController>().IsOnTrain) fixedTrainY = transform.position.y;

        if (player.GetComponent<Boots>().BootsActive && !player.GetComponent<PlayerController>().IsOnTrain || isJumpingWithBoots)
        {
            cameraPos = new Vector3(player.transform.position.x + offsetX, player.transform.position.y + offsetY, player.transform.position.z + offsetZ);
        }
        else if (player.GetComponent<PlayerController>().IsOnTrain || player.transform.position.y >= fixedTrainY - offsetY)
        {
            cameraPos = new Vector3(player.transform.position.x + offsetX, fixedTrainY, player.transform.position.z + offsetZ);
        }
        else
        {
            cameraPos = new Vector3(player.transform.position.x + offsetX, fixedY, player.transform.position.z + offsetZ);
        }
            transform.position = Vector3.Lerp(transform.position, cameraPos, 5f * Time.deltaTime);
    }
}
