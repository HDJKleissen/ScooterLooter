using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventPopup : MonoBehaviour
{
    public TextMeshProUGUI EventFlavorText, EventNameText;
    public Transform EventChoicesPanel;
    public GameObject ChoiceButtonPrefab;
    public Image EventImage;

    public void StartPopup(EventData eventData)
    {
        EventNameText.text = eventData.EventName;
        EventFlavorText.text = eventData.EventFlavor;
        EventImage.sprite = Resources.Load<Sprite>(eventData.EventImage);

        foreach(EventChoiceData choice in eventData.EventChoices)
        {
            EventChoiceButton choiceButton = Instantiate(ChoiceButtonPrefab, EventChoicesPanel).GetComponent<EventChoiceButton>();
            choiceButton.SetupChoice(choice);
        }
    }
}
