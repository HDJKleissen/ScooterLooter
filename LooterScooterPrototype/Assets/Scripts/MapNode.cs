using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour
{
    public int index;
    public MapNode parent;
    public List<MapNode> connected = new List<MapNode>();
    public bool looted;
    public bool hasEvent;
    public float eventLocation;

    private void OnMouseOver()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && MapGameController.Instance.target == parent)
        {
            MapGameController.Instance.target = this;
        }
    }
}
