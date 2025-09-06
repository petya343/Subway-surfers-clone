using UnityEngine;
public interface IGameManager
{
    bool IsGameRunning { get; set; }
    void GameOver();
}
