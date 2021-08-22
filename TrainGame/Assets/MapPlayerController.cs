using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPlayerController : MonoBehaviour
{
    [SerializeField]
    float Speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(MapGameController.Instance.Target != null && transform.position != MapGameController.Instance.Target.transform.position)
        {
            transform.Translate((MapGameController.Instance.Target.transform.position - transform.position).normalized * Speed * Time.deltaTime);
            MapGameController.Instance.Travelling = true;
            if (transform.position == MapGameController.Instance.Target.transform.position)
            {
                MapGameController.Instance.Target.visited = true;
                MapGameController.Instance.Travelling = false;
                MapGameController.Instance.Target.DrawRoads();
            }
        }
    }
}
