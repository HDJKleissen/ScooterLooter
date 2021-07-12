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
        if(MapGameController.Instance.target != null && transform.position != MapGameController.Instance.target.transform.position)
        {
            transform.Translate((MapGameController.Instance.target.transform.position - transform.position).normalized * Speed * Time.deltaTime);
            MapGameController.Instance.travelling = true;
            if (transform.position == MapGameController.Instance.target.transform.position)
            {
                MapGameController.Instance.target.travelled = true;
                MapGameController.Instance.travelling = false;
                MapGameController.Instance.target.DrawRoads();
            }
        }
    }
}
