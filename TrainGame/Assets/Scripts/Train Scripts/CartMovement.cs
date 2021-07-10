using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartMovement : MonoBehaviour
{
    public Waypoint CurrentGoal;
    public float SpeedPerSec;

    // Update is called once per frame
    void Update()
    {
        if(CurrentGoal != null)
        {
            var movement = SpeedPerSec * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position ,CurrentGoal.gameObject.transform.position, movement);
            if(CurrentGoal.transform.position == transform.position)
            {
                if(CurrentGoal.DestroyCartOnContact)
                {
                   Destroy(gameObject);
                }
                if(CurrentGoal.nextWaypoint != null)
                {
                    CurrentGoal = CurrentGoal.nextWaypoint;
                }
            }
        }
    }
}
