using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : UnitySingleton<GameController>
{
    public PlayerController Player;

    public TrainController Train;

    public GameObject EventPopupPrefab, EventPopupGO;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Train != null && Player != null)
        {
            if (Train.IsStopped && !Player.gameObject.activeInHierarchy)
            {
                Player.gameObject.SetActive(true);
            }
        }
    }

    public void CreateEventPopup(EventData eventData)
    {
        EventPopupGO = Instantiate(EventPopupPrefab);
        EventPopupGO.GetComponent<EventPopup>().StartPopup(eventData);
    }

    public void DestroyEventPopup()
    {
        Destroy(EventPopupGO.gameObject);
        EventPopupGO = null;
    }

    public void GameOver()
    {
        SceneManager.LoadScene("RunLossScene");
    }
}
