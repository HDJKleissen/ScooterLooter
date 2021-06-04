using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActorController : MonoBehaviour
{
    public float MoveSpeed;
    public float AttackDamage;
    public float MaxHealth;
    public float CurrentHealth;

    protected Vector3 movement;

    Rigidbody actorRigidbody;

    // Start is called before the first frame update
    void Awake()
    {
        actorRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        DoAttacking();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        movement = GetMovement();
        actorRigidbody.MovePosition(transform.position + movement.normalized * MoveSpeed * Time.fixedDeltaTime);
    }

    protected abstract Vector3 GetMovement();

    protected abstract void DoAttacking();

    public virtual void DealDamage(float damage)
    {
        CurrentHealth -= damage;

        if(CurrentHealth < 0)
        {
            gameObject.SetActive(false);
        }
    }

}
