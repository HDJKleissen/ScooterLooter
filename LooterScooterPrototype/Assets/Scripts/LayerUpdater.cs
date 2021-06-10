using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerUpdater : MonoBehaviour
{
    SpriteRenderer renderer;
    Collider2D collider;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        renderer.sortingOrder = -(int)(collider.bounds.center.y*100);
    }
}
