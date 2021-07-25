using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapData
{
    public Vector3 PlayerPosition;
    public int target;
    public List<MapNodeData> Nodes;
}

[Serializable]
public struct MapNodeData
{
    public int index;
    public Vector3 position;
    public bool visited;
    public bool looted;
    public List<int> children;
}
