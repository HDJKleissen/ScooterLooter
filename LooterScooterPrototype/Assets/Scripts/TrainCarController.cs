using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainCarController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pickup"))
        {
            GameObject other = collision.gameObject;
            other.GetComponent<Collider2D>().enabled = false;

            Pickup pickup = other.GetComponent<Pickup>();
            switch (pickup.type)
            {
                case Pickup.ResourceType.Ammunition:
                    GameController.Instance.Ammo += pickup.value;
                    break;
                case Pickup.ResourceType.Food:
                    GameController.Instance.Food += pickup.value;
                    break;
                case Pickup.ResourceType.Fuel:
                    GameController.Instance.Fuel += pickup.value;
                    break;
                case Pickup.ResourceType.Teddy:
                    GameController.Instance.Teddy += pickup.value;
                    break;
            }

            Shrink shrink = other.AddComponent<Shrink>();
            shrink.duration = .75f;
            shrink.StartShrink();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && LooterGameController.Instance.time <= 0)
        {
            LooterGameController.Instance.LoadMapScene();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            LooterGameController.Instance.timerStarted = true;
        }
    }
}
