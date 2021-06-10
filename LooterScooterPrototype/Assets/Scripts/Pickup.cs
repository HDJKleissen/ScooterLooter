using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public ResourceType type;
    public int value;

    public enum ResourceType {Trash, Fuel, Ammunition, Food, Teddy}
}
