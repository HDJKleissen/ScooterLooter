using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MapGameController : MonoBehaviour
{
    public static MapGameController Instance;
    public MapNode target;

    [SerializeField]
    Transform player;
    [SerializeField]
    MapNode nodePrefab;
    [SerializeField]
    Vector2Int nodeRanges;
    [SerializeField]
    Button LootButton;
    [SerializeField]
    TextMeshProUGUI foodUI, ammoUI, fuelUI, teddyUI;
    [SerializeField]
    Material lineMaterial;
    [SerializeField]
    float playerSpeed, foodConsumption, fuelConsumption;

    List<MapNode> nodes = new List<MapNode>();
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
    }

    // Update is called once per frame
    void Update()
    {
        foodUI.text = GameController.Instance.Food.ToString("0.0");
        fuelUI.text = GameController.Instance.Fuel.ToString("0.0");
        ammoUI.text = GameController.Instance.Ammo.ToString("0");
        teddyUI.text = GameController.Instance.Teddy.ToString("0");
        //teddy is unique, don't spoil it
        if (GameController.Instance.Teddy > 0)
        {
            teddyUI.transform.parent.gameObject.SetActive(true);
        }

        if(player.transform.position != target.transform.position && GameController.Instance.Food > 0 && GameController.Instance.Fuel > 0)
        {
            if ((target.transform.position - player.transform.position).magnitude < playerSpeed * Time.deltaTime)
                player.transform.position = target.transform.position;
            else
                player.transform.position += (target.transform.position - player.transform.position).normalized * playerSpeed * Time.deltaTime;
            GameController.Instance.Food = Mathf.Clamp(GameController.Instance.Food - Time.deltaTime * foodConsumption, 0, float.MaxValue);
            GameController.Instance.Fuel = Mathf.Clamp(GameController.Instance.Fuel - Time.deltaTime * fuelConsumption, 0, float.MaxValue);
            //Check if an event is passed
            if(target.hasEvent)
            {
                if(player.transform.position.x > target.eventLocation)
                {
                    target.hasEvent = false;
                    LoadDecisionScene();
                }
            }
        }

        LootButton.gameObject.SetActive(player.transform.position == target.transform.position && !target.looted);

        if(player.transform.position == target.transform.position && target.connected.Count == 0)
        {
            PopulateNodeChildren(target);
        }
    }

    public void EndRun()
    {
        File.Delete(Application.persistentDataPath + "/run.dat");
        SceneManager.LoadScene("Menu");
    }

    public void LoadLootingScene()
    {
        target.looted = true;
        Save();
        SceneManager.LoadScene("LootingScene");
    }

    public void LoadDecisionScene()
    {
        Save();
        SceneManager.LoadScene("DecisionScene");
    }

    void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/run.dat"))
        {
            string data = File.ReadAllText(Application.persistentDataPath + "/run.dat");
            SaveData deserialized = JsonUtility.FromJson<SaveData>(data);
            for(int i = 0; i < deserialized.nodes.Length; i++)
            {
                MapNode node =Instantiate(nodePrefab);
                nodes.Add(node);
                node.index = i;
                node.transform.position = deserialized.nodes[i].pos;
                node.looted = deserialized.nodes[i].looted;
                node.hasEvent = deserialized.nodes[i].hasEvent;
                node.eventLocation = deserialized.nodes[i].eventlocation;
            }
            for(int i = 0; i < deserialized.nodes.Length; i++)
            {
                if(deserialized.nodes[i].parent != -1)
                {
                    nodes[i].parent = nodes[deserialized.nodes[i].parent];
                    nodes[deserialized.nodes[i].parent].connected.Add(nodes[i]);

                    LineRenderer Line = new GameObject("Line").AddComponent<LineRenderer>();
                    Line.positionCount = 2;
                    Line.SetPosition(0, nodes[i].transform.position);
                    Line.SetPosition(1, nodes[deserialized.nodes[i].parent].transform.position);
                    Line.startWidth = .2f;
                    Line.endWidth = .2f;
                    Line.material = lineMaterial;
                }
            }

            target = nodes[deserialized.nodepos];
            player.transform.position = deserialized.playerpos;
            GameController game = GameController.Instance;
            game.Ammo = deserialized.Ammo;
            game.Food = deserialized.Food;
            game.Fuel = deserialized.Fuel;
            game.Teddy = deserialized.Teddy;
        }
        else
        {
            GameController game = GameController.Instance;
            target = Instantiate(nodePrefab);
            nodes.Add(target);
            target.index = 0;
            target.transform.position = Vector3.zero;
            player.transform.position = Vector3.zero;
            PopulateNodeChildren(target);
        }
    }

    void Save()
    {
        NodeSaveData[] nodeSaves = new NodeSaveData[nodes.Count];
        for(int i = 0; i < nodes.Count; i++)
        {
            nodeSaves[i] = new NodeSaveData
            {
                index = i,
                parent = nodes[i].parent == null ? -1 : nodes[i].parent.index,
                pos = nodes[i].transform.position,
                looted = nodes[i].looted,
                hasEvent = nodes[i].hasEvent,
                eventlocation = nodes[i].eventLocation
            };
        }
        SaveData data = new SaveData
        {
            nodes = nodeSaves,
            Food = GameController.Instance.Food,
            Fuel = GameController.Instance.Fuel,
            Ammo = GameController.Instance.Ammo,
            Teddy = GameController.Instance.Teddy,
            nodepos = target.index,
            playerpos = player.transform.position
        };
        File.WriteAllText(Application.persistentDataPath + "/run.dat", JsonUtility.ToJson(data));
    }

    void PopulateNodeChildren(MapNode node)
    {
        int children = Random.Range(1, 4);
        for(int i = 1; i < children + 1; i++)
        {
            MapNode child = Instantiate(nodePrefab);
            nodes.Add(child);
            child.index = nodes.Count - 1;
            int remainder = (i) % 2;
            int whole = Mathf.CeilToInt((i-1) / 2f);
            int y = 0 + ((remainder == 0) ? -whole : whole) * 3;
            child.transform.position = node.transform.position + new Vector3(Random.Range(nodeRanges.x, nodeRanges.y), y);
            child.parent = node;
            node.connected.Add(child);
            LineRenderer Line = new GameObject("Line").AddComponent<LineRenderer>();
            Line.positionCount = 2;
            Line.SetPosition(0, node.transform.position);
            Line.SetPosition(1, child.transform.position);
            Line.startWidth = .2f;
            Line.endWidth = .2f;
            Line.material = lineMaterial;
            //Generate event
            if(Random.Range(0, 1f) < .8f)
            {
                child.hasEvent = true;
                child.eventLocation = Random.Range(node.transform.position.x, child.transform.position.x);
            }
        }
    }
}

[Serializable]
public struct SaveData
{
    public NodeSaveData[] nodes;
    public int Ammo, Teddy;
    public float Food, Fuel;
    public int nodepos;
    public Vector3 playerpos;
}

[Serializable]
public struct NodeSaveData
{
    public int index;
    public int parent;
    public Vector3 pos;
    public bool looted;
    public bool hasEvent;
    public float eventlocation;
}
