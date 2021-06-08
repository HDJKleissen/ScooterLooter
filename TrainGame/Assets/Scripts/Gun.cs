using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public Vector3 ShootFromPosition;
    public GameObject BulletPrefab;
    public int MaxShots, ShotsRemaining;
    public float TimeBetweenShots;
    public float TimeBetweenReloads;
    public float BulletSpeed;

    float shootTimer = 0, reloadTimer = 0;
    
    private void Update()
    {
        if (shootTimer > 0)
        {
            shootTimer -= Time.deltaTime;
        }
        if (reloadTimer > 0)
        {
            reloadTimer -= Time.deltaTime;
            if(reloadTimer <= 0)
            {
                ShotsRemaining = MaxShots;
                Debug.Log("Reload");
            }
        }
    }

    public void Fire()
    {
        if (ShotsRemaining > 0 && shootTimer <= 0)
        {
            FireGun();

            ShotsRemaining--;
            shootTimer = TimeBetweenShots;
        }
        else if(ShotsRemaining <= 0 && reloadTimer <= 0)
        {
            reloadTimer = TimeBetweenReloads;
        }
    }

    protected abstract void FireGun();
}
