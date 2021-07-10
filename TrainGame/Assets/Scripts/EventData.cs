using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventData
{
    public string EventID;
    public string EventName;
    public string EventFlavor;
    public string EventImage;
    public EventChoiceData[] EventChoices;
}

public class EventChoiceData
{
    public string EventChoiceText;
    public EventRewardData[] EventRewards;
}


// Can only reward resources for now, add other stuff later.
public class EventRewardData
{
    public Dictionary<Resource.ResourceType, int> Rewards;
}