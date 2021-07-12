using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenSprite : MonoBehaviour
{
    public Vector3 RotateVector;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(RotateVector * Time.deltaTime);
    }
}
