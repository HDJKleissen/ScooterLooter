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
