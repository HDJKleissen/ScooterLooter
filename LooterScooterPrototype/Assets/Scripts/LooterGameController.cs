using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;
using System.IO;
using UnityEngine.SceneManagement;

public class LooterGameController : MonoBehaviour
{
    public static LooterGameController Instance;

    public float time = 60;
    public bool timerStarted = false;
    public int amountOfItems = 20;

    [SerializeField]
    Image timerImage;
    [SerializeField]
    TextMeshProUGUI timerText;
    [SerializeField]
    TextMeshProUGUI foodUI, ammoUI, fuelUI, teddyUI;

    [SerializeField]
    List<SpawnType> spawnables;

    private void Awake()
    {
        //Singleton Instance
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Load();
        SpawnItems();
    }

    private void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/run.dat"))
        {
            string data = File.ReadAllText(Application.persistentDataPath + "/run.dat");
            SaveData deserialized = JsonUtility.FromJson<SaveData>(data);
            GameController game = GameController.Instance;
            game.Ammo = deserialized.Ammo;
            game.Food = deserialized.Food;
            game.Fuel = deserialized.Fuel;
            game.Teddy = deserialized.Teddy;
        }
        else
        {
            GameController game = GameController.Instance;
        }
    }

    private void Save()
    {
        if (File.Exists(Application.persistentDataPath + "/run.dat"))
        {
            string data = File.ReadAllText(Application.persistentDataPath + "/run.dat");
            SaveData deserialized = JsonUtility.FromJson<SaveData>(data);
            GameController game = GameController.Instance;
            deserialized.Ammo = game.Ammo;
            deserialized.Food = game.Food;
            deserialized.Fuel = game.Fuel;
            deserialized.Teddy = game.Teddy;
            data = JsonUtility.ToJson(deserialized);
            File.WriteAllText(Application.persistentDataPath + "/run.dat", data);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(timerStarted && time > 0)
        {
            time -= Time.deltaTime;
            timerText.text = ((int)time).ToString();
            timerImage.fillAmount = time / 30;
        }

        foodUI.text = GameController.Instance.Food.ToString("0.0");
        fuelUI.text = GameController.Instance.Fuel.ToString("0.0");
        ammoUI.text = GameController.Instance.Ammo.ToString("0");
        teddyUI.text = GameController.Instance.Teddy.ToString("0");
        //teddy is unique, don't spoil it
        if(GameController.Instance.Teddy > 0)
        {
            teddyUI.transform.parent.gameObject.SetActive(true);
        }
    }

    public void LoadMapScene()
    {
        Save();
        SceneManager.LoadScene("MapScene");
    }

    void SpawnItems()
    {
        List<PickupSpawnSpot> spots = new List<PickupSpawnSpot>(FindObjectsOfType<PickupSpawnSpot>());
        amountOfItems = Mathf.Clamp(amountOfItems, 0, spots.Count);
        for (int i = 0; i < amountOfItems; i++)
        {
            PickupSpawnSpot spot = spots[Random.Range(0, spots.Count)];

            //Pick a random spawntype from a weighted list
            SpawnType result = spawnables[0];
            float randomrange = 0;
            foreach (SpawnType type in spawnables)
                randomrange += type.frequency;
            float random = Random.Range(0, randomrange);
            float chance = 0;
            for (int j = 0; j < spawnables.Count; j++)
            {
                chance += spawnables[j].frequency;
                if (random < chance)
                {
                    result = spawnables[j];
                    break;
                }
            }

            GameObject pickup = Instantiate(result.PickupPrefab);
            if (result.unique)
                spawnables.Remove(result);
            pickup.transform.position = spot.transform.position;
            spots.Remove(spot);
            Destroy(spot.gameObject);
        }
    }

    [Serializable]
    struct SpawnType
    {
        public GameObject PickupPrefab;
        public float frequency;
        public bool unique;
    }
}
