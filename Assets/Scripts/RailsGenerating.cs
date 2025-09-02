using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailsGenerating : MonoBehaviour
{
    private float startPoint;
   
    private GameObject rails;
    private GameObject player;
    private GameObject railPrefab;
    private List<GameObject> spawnedRails = new List<GameObject>();

    private Vector3 pos;
    private float railLength = 4;
    private bool isPaused = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        rails = GameObject.Find("rails");
        railPrefab = Resources.Load<GameObject>("Prefabs/rails");

        startPoint = rails.transform.position.z + railLength;
        for (int i = 0; i < 6; i++)
        {
            SpawnRail();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isPaused) return;
        if (player.transform.position.z > startPoint - 150)
        {
            SpawnRail();

        }
        DestroyRail();
    }

    private void SpawnRail()
    {
        pos = new Vector3(rails.transform.position.x, rails.transform.position.y, startPoint);
        GameObject newRail = Instantiate(railPrefab, pos, Quaternion.identity);
        spawnedRails.Add(newRail);
        startPoint += railLength;
        
    }

    private void DestroyRail()
    {
        if (spawnedRails[0].transform.position.z < player.transform.position.z - 100)
        {
            Destroy(spawnedRails[0]);
            spawnedRails.RemoveAt(0);
        }
    }
    public void Pause() => isPaused = true;
    public void Resume() => isPaused = false;

}
