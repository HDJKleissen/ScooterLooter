using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrink : MonoBehaviour
{
    public float duration;

    Coroutine routine;
    public void StartShrink(bool destroyAfterShrunk = true)
    {
        if (routine != null)
            StopCoroutine(routine);
        StartCoroutine(ShrinkRoutine());
        if (destroyAfterShrunk)
            Destroy(gameObject, duration);
    }

    IEnumerator ShrinkRoutine()
    {
        float time = 0;
        while (time < 1)
        {
            time += Time.deltaTime / duration;
            transform.localScale = new Vector3(1 - time, 1 - time, 1 - time);
            yield return null;
        }
    }
}
