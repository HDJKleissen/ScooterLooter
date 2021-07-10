using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class LayerUpdater : MonoBehaviour
{
    [SerializeField]
    bool StaticObject;

    SpriteRenderer renderer;
    Collider2D collider;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();

        if (collider != null)
            renderer.sortingOrder = -(int)(collider.bounds.center.y * 10);
        else
            renderer.sortingOrder = -(int)(transform.position.y * 10);
        if (StaticObject)
            Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (collider != null)
            renderer.sortingOrder = -(int)(collider.bounds.center.y * 10);
        else
            renderer.sortingOrder = -(int)(transform.position.y * 10);
    }
}
