using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem : MonoBehaviour, IScoreSystem
{
    public static ScoreSystem Instance { get; private set; }

    private int score = 0;
    private float startZ;
    private void Awake()
    {
        Instance = this;

    }
    private void Start()
    {
        startZ = transform.position.z;
    }

    public void UpdateScore()
    {
        score = Mathf.RoundToInt(transform.position.z - startZ);
        UIManager.Instance.UpdateScore(score);
    }
}
