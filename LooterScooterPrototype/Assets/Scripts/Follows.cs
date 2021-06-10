using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follows : MonoBehaviour
{
    public Transform target;
    public bool X, Y, Z;

    private void Update()
    {
        transform.position = new Vector3(X ? target.position.x: transform.position.x, Y ? target.position.y : transform.position.y, Z ? target.position.z : transform.position.z);
    }
}
