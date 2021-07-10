using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventChoiceButton : MonoBehaviour
{
    public TextMeshProUGUI text;

    EventChoiceData eventChoiceData;

    public void SetupChoice(EventChoiceData eventChoiceData)
    {
        this.eventChoiceData = eventChoiceData;

        text.text = eventChoiceData.EventChoiceText;
        foreach(EventRewardData eventRewardData in eventChoiceData.EventRewards)
        {
            text.text += "\n(" + RewardText(eventChoiceData.EventRewards[0]) + ")";
        }
    }

    string RewardText(EventRewardData rewardData)
    {
        string returnString = "";

        int resourcesAdded = 0;
        foreach (KeyValuePair<Resource.ResourceType, int> kvp in rewardData.Rewards)
        {
            string colorTag = "", colorEndTag = "";
            if (kvp.Value < 0)
            {
                colorTag = "<color=#FF0000>";
                colorEndTag = "</color>";
            }
            if (kvp.Value > 0)
            {
                colorTag = "<color=#00FF00>+";
                colorEndTag = "</color>";
            }
            
            returnString += $"{colorTag}{kvp.Value} {kvp.Key}{colorEndTag}";

            resourcesAdded++; 

            if(resourcesAdded < rewardData.Rewards.Count)
            {
                returnString += ", ";
            }
        }

        return returnString;
    }

    public void DoOnClick()
    {
        foreach (EventRewardData eventRewardData in eventChoiceData.EventRewards)
        {
            foreach (KeyValuePair<Resource.ResourceType, int> kvp in eventRewardData.Rewards)
            {
                GameController.Instance.Data.AddResource(kvp.Key, kvp.Value);
            }
        }

        GameController.Instance.DestroyEventPopup();
    }
}