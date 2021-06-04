using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDirector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static ZombieBehaviours GetBehaviour(ZombieController controller)
    {
        if (GameController.Instance.Player.gameObject.activeInHierarchy &&
            Vector3.Distance(controller.transform.position, GameController.Instance.Player.transform.position) < 8)
        {
            return ZombieBehaviours.FOLLOW_PLAYER;
        }
        else if (GameController.Instance.Train.gameObject.activeInHierarchy && GameController.Instance.Train.IsStopped &&
            Vector3.Distance(controller.transform.position, GameController.Instance.Train.transform.position) < 15)
        {
            return ZombieBehaviours.ATTACK_TRAIN;
        }
        else if (GameController.Instance.Train.IsStopped)
        {
            return ZombieBehaviours.IDLE;
        }
        return ZombieBehaviours.STAND_STILL;
    }
}

// Swap out with classes for fancy shmancy states
public enum ZombieBehaviours
{
    STAND_STILL,
    IDLE,
    ATTACK_TRAIN,
    FOLLOW_PLAYER
}