using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 Direction;
    public float Damage;
    public float BulletSpeed;
    public ActorController Owner;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CoroutineHelper.DelaySeconds(() => Destroy(gameObject), 5));
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Direction * BulletSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        ActorController other = collision.collider.GetComponent<ActorController>();
        if(other != null && other != Owner)
        {
            other.DealDamage(Damage);
        }
    }
}
