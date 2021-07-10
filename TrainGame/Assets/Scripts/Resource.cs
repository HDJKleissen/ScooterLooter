using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : PickupItem
{
    public ResourceType type;
    public int value;

    public enum ResourceType { Fuel, Ammunition, Food }
}
