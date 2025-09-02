using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class ObstaclesGenerating : MonoBehaviour
{
    [Header("Trains")]
    public GameObject[] trainsPrefabs;

    [Header("Jump/Slide obstacles")]
    public GameObject[] obstaclesPrefabs;

    private List<GameObject> AllObstacles = new List<GameObject>();
    private List<bool> blockedLines = new List<bool>();
    private List<float> endPoints = new List<float>();
    private GameObject player;

    private int[] lines = { -1, 0, 1 };
    private float[] coordinateX = { -0.8f, 3.2f, 7.2f };
    private float maxEndPoint;
    private float startPoint;
    private int freeLines = 3;
    private Vector3 lastPosition;
    private List<float> respawnPoints = new List<float>();
    private bool isPaused = false;
    private float fixedY;
    private bool spawnedHeart = false;
    private Vector3 spawnedHeartPos = Vector3.zero;

    void Start()
    {
        player = GameObject.Find("Player");
        blockedLines = new List<bool> { false, false, false };
        for (int i = 0; i < 3; ++i)
        {
            endPoints.Add(player.transform.position.z + 30f);
        }
        startPoint = player.transform.position.z + 30f;
        lastPosition = player.transform.position;
        respawnPoints.Add(startPoint);
        fixedY = player.transform.position.y;

    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused) return;

        if (player.transform.position.z > respawnPoints[0])
        {
            respawnPoints.RemoveAt(0);
        }

        lastPosition = new Vector3(coordinateX[1], fixedY, respawnPoints[0] - 9f);

        if (spawnedHeartPos.z < transform.position.z)
        {
            spawnedHeart = false;
        }

        if (player.transform.position.z > maxEndPoint - 150)
        {
            freeLines = 3;
            blockedLines = new List<bool> { false, false, false };

            TrainsSpawn();
            ObstaclesSpawn();

            startPoint = maxEndPoint + 15f;
            respawnPoints.Add(startPoint);

        }
        DestroyObstacles();
    }

    private void TrainsSpawn()
    {
        int trains = Random.Range(0, 3);
        int placedTrains = 0;
        GameObject train;
        Collider trainCol;

        while (placedTrains < trains)
        {
            int line = Random.Range(-1, 2);

            if (!blockedLines[line + 1] && freeLines > 1)
            {
                Vector3 pos = new Vector3(coordinateX[line + 1],
                                          0.2f,
                                          startPoint);

                train = Instantiate(trainsPrefabs[Random.Range(0, trainsPrefabs.Length)],
                                    pos,
                                    Quaternion.identity);

                AllObstacles.Add(train);

                trainCol = train.GetComponent<Collider>();

                endPoints[line + 1] = startPoint + trainCol.bounds.size.z;

                placedTrains++;
                freeLines--;
                blockedLines[line + 1] = true;

                if (maxEndPoint < endPoints[line + 1])
                {
                    maxEndPoint = endPoints[line + 1];
                }

                float spawnCoins = Random.value;
                if (spawnCoins > 0.5f && !train.name.Contains("train-"))
                {
                    GameObject coinPrefab = Resources.Load<GameObject>("Prefabs/coin");
                    for (float i = startPoint + 2.5f; i < endPoints[line + 1]; i += 2.5f)
                    {
                        Vector3 coinPos = new Vector3(train.transform.position.x,
                                                      train.transform.position.y + trainCol.bounds.size.y + 2f,
                                                      i);

                        Instantiate(coinPrefab, coinPos, Quaternion.identity);
                    }
                }
            }
        }
        //placedTrains = 0;
    }

    private void ObstaclesSpawn()
    {
        int obstacles = Random.Range(1, freeLines + 1);
        GameObject obstacle;
        int placedObstacles = 0;

        bool power = false;
        float onlyOnePower = 0;
        float spawnPower = Random.value;
        if (spawnPower <= 0.4f)
        {
            power = true;
        }

        while (placedObstacles < obstacles && freeLines > 0) //>=0
        {
            int line = Random.Range(-1, 2);
            if (!blockedLines[line + 1])
            {
                if (maxEndPoint < startPoint) maxEndPoint = startPoint + 10f;
                float ObstacleStartPoint = Random.Range(startPoint, maxEndPoint); //maxend - 5f

                Vector3 pos = new Vector3(coordinateX[line + 1],
                                          -0.3f,
                                          ObstacleStartPoint);

                obstacle = Instantiate(obstaclesPrefabs[Random.Range(0, obstaclesPrefabs.Length)],
                                       pos,
                                       Quaternion.identity);

                AllObstacles.Add(obstacle);

                if (obstacle.gameObject.name.Contains("jump obstacle"))
                {
                    obstacle.transform.Rotate(0f, 90f, 0f);
                }

                placedObstacles++;
                freeLines--;
                blockedLines[line + 1] = true;

                endPoints[line + 1] = ObstacleStartPoint + 5f; //startPoint
                if (maxEndPoint < endPoints[line + 1])
                {
                    maxEndPoint = endPoints[line + 1];
                }

                float spawnCoins1 = Random.value;

                float begin, end;
                if (ObstacleStartPoint - startPoint > maxEndPoint - 2f - ObstacleStartPoint)
                {
                    begin = startPoint;
                    end = ObstacleStartPoint - 1f;
                }
                else
                {
                    begin = ObstacleStartPoint + 1f;
                    end = maxEndPoint;
                }

                if (power)
                {
                    float ChoosePower = Random.value;
                    Vector3 powerPos = new Vector3(coordinateX[line + 1], obstacle.transform.position.y + 1f, begin + 2f);
                    begin += 3f;
                    GameObject powerPrefab;

                    if (ChoosePower <= 0.45)
                    {
                        Instantiate(Resources.Load<GameObject>("Prefabs/Magnet"), powerPos, Quaternion.identity);
                    }
                    else if (ChoosePower > 0.45 && ChoosePower <= 0.9f)
                    {
                        powerPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/Boots"), powerPos, Quaternion.identity);
                        powerPrefab.transform.Rotate(0f, 90f, 0f);
                    }
                    else if (!player.GetComponent<ExtraLife>().HasExtraLife() && !spawnedHeart)
                    {
                        Instantiate(Resources.Load<GameObject>("Prefabs/Heart"), powerPos, Quaternion.identity);
                        spawnedHeartPos = powerPos;
                        spawnedHeart = true;
                    }
                    power = false;
                    onlyOnePower++;
                }

                if (spawnCoins1 >= 0.5f)
                {
                    if (obstacle.gameObject.transform.childCount == 0) //transform.childCount == 0
                    {
                        GameObject coinPrefab = Resources.Load<GameObject>("Prefabs/coin");
                        for (float i = begin + 2.5f; i < end; i += 2.5f)
                        {
                            Vector3 coinPos = new Vector3(obstacle.transform.position.x, obstacle.transform.position.y + 1f, i);
                            Instantiate(coinPrefab, coinPos, Quaternion.identity);
                        }
                    }
                }
            }
        }

        float spawnPower2 = Random.value;
        bool power2 = false;
        if (onlyOnePower == 0)
        {
            if (spawnPower2 <= 0.4f)
            {
               power2 = true;
            }
        }
        float spawnCoins2 = Random.value;
        if (freeLines > 0)
        {
            for (int i = 0; i < 3; i++)
            {
                if (!blockedLines[i])
                {
                    if (power2)
                    {
                        float ChoosePower = Random.value;
                        Vector3 powerPos = new Vector3(coordinateX[i], 0.7f, startPoint + 2f);
                        startPoint += 3f;
                        GameObject powerPrefab;

                        if (ChoosePower <= 0.45)
                        {
                            Instantiate(Resources.Load<GameObject>("Prefabs/Magnet"), powerPos, Quaternion.identity);
                        }
                        else if (ChoosePower > 0.45 && ChoosePower <= 0.9f)
                        {
                            powerPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/Boots"), powerPos, Quaternion.identity);
                            powerPrefab.transform.Rotate(0f, 90f, 0f);
                        }
                        else if (!player.GetComponent<ExtraLife>().HasExtraLife() && !spawnedHeart)
                        {
                            Instantiate(Resources.Load<GameObject>("Prefabs/Heart"), powerPos, Quaternion.identity);
                            spawnedHeartPos = powerPos;
                            spawnedHeart = true;
                        }
                        power2 = false;
                    }

                    if (spawnCoins2 > 0.5f)
                    {
                        GameObject coinPrefab = Resources.Load<GameObject>("Prefabs/coin");
                        for (float j = startPoint + 2.5f; j < maxEndPoint; j += 2.5f)
                        {
                            Vector3 coinPos = new Vector3(coordinateX[i], 0.2f, i);
                            Instantiate(coinPrefab, coinPos, Quaternion.identity);
                        }
                    }
                }
            }
        }
    }
        
    
        //placedObstacles = 0;
        //}

    private void DestroyObstacles()
    {
        if (AllObstacles.Count > 0 && AllObstacles[0].transform.position.z < player.transform.position.z - 100)
        {
            Destroy(AllObstacles[0]);
            AllObstacles.RemoveAt(0);
        }
    }

    public Vector3 getLastPosition()
    {
        return lastPosition;
    }
    public void Pause() => isPaused = true;
    public void Resume() => isPaused = false;

}
