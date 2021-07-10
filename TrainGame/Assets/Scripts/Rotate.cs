using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float Speed = 80;
    bool X = false, Y = false, Z = true;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(X ? Speed * Time.deltaTime : 0, Y ? Speed * Time.deltaTime : 0, Z ? Speed * Time.deltaTime : 0);
    }
}
