using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : UnitySingleton<GameController>
{
    public PlayerController Player;

    public TrainController Train;

    public GameData Data = new GameData();
    
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Train.IsStopped && !Player.gameObject.activeInHierarchy)
        {
            Player.gameObject.SetActive(true);
        }
    }
}
