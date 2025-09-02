using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class HouseGenerating : MonoBehaviour
{
    [Header("Houses")]
    public GameObject[] housesPrefabs;

    private GameObject player;
    private GameObject firstLeftHouse;
    private GameObject firstRightHouse;
    private List<GameObject> left_houses = new List<GameObject>();
    private List<GameObject> right_houses = new List<GameObject>();

    private float startLeftPoint;
    private float startRightPoint;
    private Vector3 pos;
    private float houseLength = 13f;
    private bool isPaused = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        firstLeftHouse = GameObject.Find("green house");
        startLeftPoint = firstLeftHouse.transform.position.z + houseLength;

        firstRightHouse = GameObject.Find("green house (1)");
        startRightPoint = firstRightHouse.transform.position.z + houseLength;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused) return;
        if (player.transform.position.z > startLeftPoint - 150)
        {
            SpawnLeftHouse();
            SpawnRightHouse();
        }
        DestroyHouse(left_houses);
        DestroyHouse(right_houses);
    }

    private void SpawnLeftHouse()
    {
        pos = new Vector3(firstLeftHouse.transform.position.x, firstLeftHouse.transform.position.y, startLeftPoint);
        GameObject currentPrefab = Instantiate(
            housesPrefabs[Random.Range(0, housesPrefabs.Length)], 
            pos, 
            Quaternion.identity);
        currentPrefab.transform.Rotate(0f, -90f, 0f);
        left_houses.Add(currentPrefab);
        startLeftPoint += houseLength;
    }

    private void SpawnRightHouse()
    {
        pos = new Vector3(firstRightHouse.transform.position.x, firstRightHouse.transform.position.y, startRightPoint);
        GameObject currentPrefab = Instantiate(
            housesPrefabs[Random.Range(0, housesPrefabs.Length)],
            pos,
            Quaternion.identity);
        currentPrefab.transform.Rotate(0f, 90f, 0f);
        right_houses.Add(currentPrefab);
        startRightPoint += houseLength;
    }

    private void DestroyHouse(List<GameObject> houses)
    {
        if (houses.Count > 0 && houses[0].transform.position.z < player.transform.position.z - 100)
        {
            Destroy(houses[0]);
            houses.RemoveAt(0);
        }
    }
    public void Pause() => isPaused = true;
    public void Resume() => isPaused = false;

}
