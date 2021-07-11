using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : ActorController
{
    public bool IsStopped {
        get {
            return MoveSpeed < 1f;
        }
    }

    public bool SlownDownTrain;

    bool slowingDown = false;
    protected override Vector3 GetMovement()
    {
        if (SlownDownTrain)
        {
            return SlowingDownMovement();
        }
        else
        {
            return NormalMovement();
        }
    }

    private Vector3 NormalMovement()
    {
        return new Vector3(-1, 0, 0);
    }

    private Vector3 SlowingDownMovement()
    {

        Vector3 nulledPosition = transform.position;
        nulledPosition.y = 0;

        if (!slowingDown)
        {
            if (Vector3.Distance(nulledPosition, new Vector3(10, 0, 0)) > 0.5f)
            {
                return new Vector3(-1, 0, 0);
            }
            else
            {
                slowingDown = true;
            }
        }
        MoveSpeed = Mathf.Lerp(MoveSpeed, 0, Time.fixedDeltaTime);
        return movement;
    }

    protected override void DoAttacking()
    {
        // Auto gun turrets, NPCS with guns on train etc go here
    }

    public override void DealDamage(float damage)
    {
        base.DealDamage(damage);
        GameUIController.Instance.TrainHealthUI.SetSliderValue(1 - (MaxHealth - CurrentHealth)/MaxHealth);
    }
}
