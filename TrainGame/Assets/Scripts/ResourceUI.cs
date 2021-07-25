using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(VerticalLayoutGroup))]
public class ResourceUI : MonoBehaviour
{
    [SerializeField]
    List<ResourceTypeUISettings> resourceSettings;
    [SerializeField]
    GameObject ResourceCounterPrefab;

    Dictionary<Resource.ResourceType, ResourceTypeUISettings> settings;
    Dictionary<Resource.ResourceType, ResourceUIInstance> UIInstances = new Dictionary<Resource.ResourceType, ResourceUIInstance>();
    private void Awake()
    {
        //Convert the list to a dictionary
        settings = new Dictionary<Resource.ResourceType, ResourceTypeUISettings>();
        foreach(ResourceTypeUISettings type in resourceSettings)
        {
            settings[type.Type] = type;
        }
        //Validate all types are present
        foreach(int type in Enum.GetValues(typeof(Resource.ResourceType)))
        {
            if (!settings.ContainsKey((Resource.ResourceType)type))
            {
                Debug.LogError($"No UI settings were given for the resource type {(Resource.ResourceType)type}. The UI won't know how to show it on screen!");
            }
        }
    }

    private void Start()
    {
        //Subscribe to the GameData event
        InterSceneData.OnResourceChange += UpdateUI;
        //Set initial values
        for (int i = 0; i < resourceSettings.Count; i++)
        {
            UpdateUI(resourceSettings[i].Type, InterSceneData.GetResource(resourceSettings[i].Type));
        }
    }

    void UpdateUI(Resource.ResourceType type, int amount)
    {
        Debug.Log($"Updating {type}");
        //If the resource is gone and it isn't shown by default, remove the instance icon or don't bother creating it
        if(amount <= 0 && !settings[type].ShowByDefault)
        {
            if (UIInstances.ContainsKey(type))
            {
                Destroy(UIInstances[type].parent);
                UIInstances.Remove(type);
            }
            return;
        }
        
        if(!UIInstances.ContainsKey(type))
        {
            ResourceUIInstance instance = new ResourceUIInstance();
            instance.parent = Instantiate(ResourceCounterPrefab, transform);
            instance.icon = instance.parent.GetComponentInChildren<Image>();
            instance.text = instance.parent.GetComponentInChildren<TextMeshProUGUI>();
            UIInstances.Add(type, instance);

            instance.icon.sprite = settings[type].Icon;
        }
        UIInstances[type].text.text = amount.ToString();
    }

    [Serializable]
    struct ResourceTypeUISettings
    {
        public Resource.ResourceType Type;
        public bool ShowByDefault;
        public Sprite Icon;
    }
    struct ResourceUIInstance
    {
        public GameObject parent;
        public TextMeshProUGUI text;
        public Image icon;
    }
}
