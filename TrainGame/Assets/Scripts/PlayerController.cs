using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : ActorController
{
    public Gun currentGun;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected override void Update()
    {
        base.Update();
        Vector3 mousePlayerDiff = (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized;
        //TODO: set gun held distance from player to more meaningful value
        currentGun.transform.position = transform.position + mousePlayerDiff.normalized;
    }

    protected override Vector3 GetMovement()
    {
        return new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0 ).normalized;
    }

    protected override void DoAttacking()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentGun.Fire();
        }
        //throw new System.NotImplementedException();
    }
}
