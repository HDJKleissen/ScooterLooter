using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flicker : MonoBehaviour
{
    //Light2D light;

    const float defaultFuelValue = 100;

    public float defaultMinValue = 1f;
    public float defaultMaxValue = 10;
    public float minDeviation = 0.5f;
    public float maxDeviation = 0.5f;
    public float smoothingRange = 0.5f;
    public float timeBetweenBlips = 0.1f;
    public float timeBetweenBlipsRandomness = 0.02f;

    float lastDeviation;

    // Start is called before the first frame update
    void Start()
    {
        //light = GetComponent<Light2D>();
        lastDeviation = Random.Range(-minDeviation, maxDeviation);
        StartCoroutine(FlickerRoutine());
    }


    IEnumerator FlickerRoutine()
    {
        while (true)
        {
            //float defaultStrength = defaultMinValue + Mathf.Clamp(FurnaceController.fuelValue / defaultFuelValue, 0, 1) * (defaultMaxValue - defaultMinValue);
            float deviation = Random.Range(Mathf.Max(minDeviation, lastDeviation - smoothingRange), Mathf.Min(maxDeviation, lastDeviation + smoothingRange));
            //light.pointLightOuterRadius = defaultStrength + Random.Range(-minDeviation, maxDeviation);
            yield return new WaitForSeconds(timeBetweenBlips + Random.Range(-timeBetweenBlipsRandomness, timeBetweenBlipsRandomness));
        }
    }
}
