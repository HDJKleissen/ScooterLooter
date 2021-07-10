using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrink : MonoBehaviour
{
    public void StartShrink(float time)
    {
        StartCoroutine(ShrinkRoutine(time));
    }

    IEnumerator ShrinkRoutine(float time)
    {
        float timer = time;
        Vector3 startScale = transform.localScale;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            transform.localScale = startScale * (timer / time);
            yield return null;
        }
    }
}
