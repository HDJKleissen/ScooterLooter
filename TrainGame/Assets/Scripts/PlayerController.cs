using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : ActorController
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected override Vector3 GetMovement()
    {
        return new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
    }

    protected override void DoAttacking()
    {
        //throw new System.NotImplementedException();
    }
}
