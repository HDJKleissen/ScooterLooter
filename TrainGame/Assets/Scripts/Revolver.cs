using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : Gun
{
    protected override void FireGun()
    {
        Bullet bullet = Instantiate(BulletPrefab, transform.position + ShootFromPosition, Quaternion.identity).GetComponent<Bullet>();
        bullet.BulletSpeed = BulletSpeed;

        Vector3 mousePlayerDiff = (Camera.main.WorldToScreenPoint(transform.position) - Input.mousePosition).normalized;
        Debug.Log(mousePlayerDiff);
        Debug.DrawRay(transform.position, mousePlayerDiff * 10, Color.green, 2);
        Debug.DrawRay(transform.position, new Vector3(mousePlayerDiff.x, 0, mousePlayerDiff.y) * 10, Color.red,2 );
        bullet.Direction = new Vector3(mousePlayerDiff.x, 0, mousePlayerDiff.y);
    }
}
