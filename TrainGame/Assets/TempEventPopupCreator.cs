using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEventPopupCreator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameController.Instance.CreateEventPopup(new EventData()
        {
            EventName = "W I D E C O W B O Y",
            EventFlavor = "Text that explains what the fuck is going on with some flavorino. Then some more flavorino that tells you \"wahhhhh\". This time it's wide cowboy comin at ya.",
            EventImage = "Sprites/WideCowboy",
            EventChoices = new EventChoiceData[]
            {
                new EventChoiceData()
                {
                    EventChoiceText = "Fuckin' run",
                    EventRewards = new EventRewardData[0]
                },
                new EventChoiceData()
                {
                    EventChoiceText = "Shoot him",
                    EventRewards = new EventRewardData[]
                    {
                        new EventRewardData()
                        {
                            Rewards = new Dictionary<Resource.ResourceType, int>()
                            {
                                { Resource.ResourceType.Ammunition, -1},
                                { Resource.ResourceType.Food, 15},
                                { Resource.ResourceType.Fuel, 1},
                            }
                        }
                    }
                },
                new EventChoiceData()
                {
                    EventChoiceText = "Dodge his shots (and die lmao idiot)",
                    EventRewards = new EventRewardData[]
                    {
                        new EventRewardData()
                        {
                            Rewards = new Dictionary<Resource.ResourceType, int>()
                            {
                                { Resource.ResourceType.Ammunition, -GameController.Instance.Data.GetResource(Resource.ResourceType.Ammunition)},
                                { Resource.ResourceType.Food, -GameController.Instance.Data.GetResource(Resource.ResourceType.Ammunition)},
                                { Resource.ResourceType.Fuel, -GameController.Instance.Data.GetResource(Resource.ResourceType.Ammunition)},
                            }
                        }
                    }
                },
                new EventChoiceData()
                {
                    EventChoiceText = "Give him some of your stuff",
                    EventRewards = new EventRewardData[]
                    {
                        new EventRewardData()
                        {
                            Rewards = new Dictionary<Resource.ResourceType, int>()
                            {
                                { Resource.ResourceType.Ammunition, -GameController.Instance.Data.GetResource(Resource.ResourceType.Ammunition)/3},
                                { Resource.ResourceType.Food, -GameController.Instance.Data.GetResource(Resource.ResourceType.Ammunition)/3},
                                { Resource.ResourceType.Fuel, -GameController.Instance.Data.GetResource(Resource.ResourceType.Ammunition)/3},
                            }
                        }
                    }
                },
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
