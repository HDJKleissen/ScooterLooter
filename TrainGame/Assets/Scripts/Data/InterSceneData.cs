using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InterSceneData
{
    //Resources
    public static event Action<Resource.ResourceType, int> OnResourceChange;
    //Map Data
    public static MapData Map;

    static Dictionary<Resource.ResourceType, int> Resources = new Dictionary<Resource.ResourceType, int>();


    public static void AddResource(Resource.ResourceType type, int amount)
    {
        if (Resources.ContainsKey(type))
        {
            Resources[type] = Mathf.Clamp(Resources[type] + amount, 0, int.MaxValue);
            OnResourceChange?.Invoke(type, amount);
        }
        else
        {
            Resources.Add(type, Mathf.Clamp(amount, 0, int.MaxValue));
            OnResourceChange?.Invoke(type, amount);
        }
    }

    public static int GetResource(Resource.ResourceType type)
    {
        if (!Resources.ContainsKey(type))
            return 0;
        else
            return Resources[type];
    }
}
