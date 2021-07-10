using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    //Resources
    public event Action<Resource.ResourceType, int> OnResourceChange;

    Dictionary<Resource.ResourceType, int> Resources = new Dictionary<Resource.ResourceType, int>();
    public void AddResource(Resource.ResourceType type, int amount)
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

    public int GetResource(Resource.ResourceType type)
    {
        if (!Resources.ContainsKey(type))
            return 0;
        else
            return Resources[type];
    }
}
