using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="Decision", menuName = "LooterScooter/GameDecision")]
[Serializable]
public class GameDecision : ScriptableObject
{
    public string intro;
    public DecisionChoice choice1;
    public DecisionChoice choice2;
    public bool showEnemy;
}

[Serializable]
public struct DecisionChoice
{
    public string decision;
    public string outro;
    public bool allowPartialPayment;
    public int foodcost, foodgain;
    public int fuelcost, fuelgain;
    public int teddycost, teddygain;
    public int ammocost, ammogain;
}
