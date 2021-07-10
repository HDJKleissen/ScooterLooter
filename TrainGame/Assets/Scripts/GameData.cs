using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    //Resources
    public event Action<Resource.ResourceType, float> OnResourceChange;

    Dictionary<Resource.ResourceType, float> Resources = new Dictionary<Resource.ResourceType, float>();
    public void AddResource(Resource.ResourceType type, float amount)
    {
        if (Resources.ContainsKey(type))
        {
            Resources[type] = Mathf.Clamp(Resources[type] + amount, 0, float.MaxValue);
            OnResourceChange?.Invoke(type, amount);
        }
        else
        {
            Resources.Add(type, Mathf.Clamp(amount, 0, float.MaxValue));
            OnResourceChange?.Invoke(type, amount);
        }
    }

    public float GetResource(Resource.ResourceType type)
    {
        if (!Resources.ContainsKey(type))
            return 0;
        else
            return Resources[type];
    }
}
