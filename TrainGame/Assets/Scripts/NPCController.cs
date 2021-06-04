using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPCController : ActorController
{
    public float AttackSpeed;
    protected float attackTimer = 0;
    protected bool attacking;
}
