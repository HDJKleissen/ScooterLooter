using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : ActorController
{
    public Gun currentGun;
    [SerializeField]
    float throwStrength;
    [SerializeField]
    Transform heldItemTransform;

    PickupItem heldItem;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected override void Update()
    {
        base.Update();
        HandleInput();
    }

    void HandleInput()
    {
        Vector3 mouseDirection = (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized;
        //TODO: set gun held distance from player to more meaningful value
        //Point the gun towards where the mouse is pointing
        currentGun.transform.position = transform.position + mouseDirection.normalized;

        if (Input.GetKeyDown(KeyCode.Mouse0) && heldItem != null)
            ThrowObject();

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (heldItem == null)
                TryPickup();
            else
                DropObject();
        }
    }

    void TryPickup()
    {
        //Check for pickups in a box around the player
        Vector3 center = new Vector2(transform.position.x, transform.position.y);
        Vector2 size = new Vector2(3, 2);
        Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, 0);

        //Debug draw box
        DebugHelper.DrawBox2D(center, size, Color.red, 1.5f);
        //EndDebug draw box

        float distance = float.MaxValue;
        PickupItem closest = null;
        //Check all hits in the overlap box
        foreach (Collider2D hit in hits)
        {
            //Only keep the closest hit
            PickupItem item = hit.gameObject.GetComponent<PickupItem>();
            float hitdistance = (transform.position - hit.transform.position).sqrMagnitude;
            if (item != null &&  hitdistance < distance)
            {
                distance = hitdistance;
                closest = item;
            }
        }
        if(closest != null)
        {
            PickUpObject(closest);
        }
    }

    void PickUpObject(PickupItem item)
    {
        heldItem = item;
        heldItem.GetComponent<Collider2D>().enabled = false;
        Rigidbody2D rb = heldItem.GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        heldItem.transform.SetParent(heldItemTransform);
        heldItem.transform.localPosition = Vector3.zero;
        heldItem.transform.localRotation = Quaternion.identity;
    }

    void DropObject()
    {
        heldItem.transform.position = transform.position;
        heldItem.transform.SetParent(null);
        heldItem.GetComponent<Rigidbody2D>().isKinematic = false;
        heldItem.GetComponent<Collider2D>().enabled = true;
        heldItem = null;
    }

    void ThrowObject()
    {
        Vector3 mouseDirection = (Input.mousePosition - Camera.main.WorldToScreenPoint(heldItem.transform.position)).normalized;
        heldItem.transform.SetParent(null);
        Rigidbody2D rb = heldItem.GetComponent<Rigidbody2D>();
        rb.isKinematic = false;
        rb.velocity = mouseDirection * throwStrength;
        heldItem.GetComponent<Collider2D>().enabled = true;
        heldItem = null;
    }

    protected override Vector3 GetMovement()
    {
        return new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0 ).normalized;
    }

    protected override void DoAttacking()
    {
        if (Input.GetMouseButtonDown(0) && heldItem == null)
        {
            currentGun.Fire();
        }
    }
}
