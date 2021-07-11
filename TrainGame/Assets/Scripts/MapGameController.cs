using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGameController : UnitySingleton<MapGameController>
{
    public MapNode target;
    public bool travelling;
    MapPlayerController player;
    protected override void Awake()
    {
        base.Awake();
        player = FindObjectOfType<MapPlayerController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
