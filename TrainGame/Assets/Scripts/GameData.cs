using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    //Resources
    public Dictionary<Resource.ResourceType, float> Resources = new Dictionary<Resource.ResourceType, float>();

    public void AddResource(Resource.ResourceType type, float amount)
    {
        if (Resources.ContainsKey(type))
            Resources[type] = Mathf.Clamp(Resources[type] + amount, 0, float.MaxValue);
    }
}
