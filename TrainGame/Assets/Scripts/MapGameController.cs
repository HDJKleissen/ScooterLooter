using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGameController : UnitySingleton<MapGameController>
{
    public MapNode target;
    public bool travelling;

    [Header("Prefabs")]
    [SerializeField]
    public MapNode NodePrefab;

    List<MapNode> nodes;
    MapPlayerController player;
    protected override void Awake()
    {
        base.Awake();
        player = FindObjectOfType<MapPlayerController>();

        //If the game saved data from a previous visit on the map, update the map with the data
        if (InterSceneData.Map != null)
        {
            Load();
        }
        else
            nodes = new List<MapNode>(FindObjectsOfType<MapNode>());
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Load()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        nodes = new List<MapNode>();
        //Set the player to the last known position
        player.transform.position = InterSceneData.Map.PlayerPosition;
        //Instantiate map nodes with the correct data
        foreach (MapNodeData node in InterSceneData.Map.Nodes)
        {
            MapNode instance = Instantiate(NodePrefab, node.position, Quaternion.identity, transform);
            nodes.Add(instance);
            instance.index = node.index;
            if (instance.index == InterSceneData.Map.target)
                target = instance;
            instance.visited = node.visited;
            instance.looted = node.looted;
        }
        //Set children and parents relationships
        foreach (MapNodeData data in InterSceneData.Map.Nodes)
        {
            MapNode node = nodes.Find((x) => x.index == data.index);
            for (int i = 0; i < data.children.Count; i++)
                node.children.Add(nodes.Find((x) => x.index == data.children[i]));
            //Debug.Log($"node {node.index} {node.children.Count}");
        }
    }

    public void Save()
    {
        List<MapNodeData> nodeData = new List<MapNodeData>();
        foreach (MapNode node in nodes)
        {
            List<int> children = new List<int>();
            foreach (MapNode child in node.children)
                children.Add(child.index);
            nodeData.Add(new MapNodeData
            {
                position = node.transform.position,
                visited = node.visited,
                looted = node.looted,
                index = node.index,
                children = children
            });
        }
        InterSceneData.Map = new MapData
        {
            PlayerPosition = player.transform.position,
            Nodes = nodeData,
            target = target.index
        };
    }
}
