using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TrainResourceCollector : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Resource resource = collision.gameObject.GetComponent<Resource>();
        if(resource != null)
        {
            GameController.Instance.Data.AddResource(resource.type, resource.value);
            resource.gameObject.GetComponent<Collider2D>().enabled = false;
            resource.gameObject.AddComponent<Shrink>().StartShrink(1);
            resource.gameObject.AddComponent<Rotate>();
            Destroy(resource.gameObject, 1);
        }
    }
}
