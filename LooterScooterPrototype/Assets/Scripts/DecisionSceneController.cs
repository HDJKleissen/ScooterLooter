using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class DecisionSceneController : MonoBehaviour
{
    [SerializeField]
    float secondPerCharacterScroll = .03f;
    [SerializeField]
    TextMeshProUGUI foodUI, ammoUI, fuelUI, teddyUI;
    [SerializeField]
    TextMeshProUGUI decisiondescription;
    [SerializeField]
    TextMeshProUGUI postdecisiondescription;
    [SerializeField]
    Button decision1Button, decision2Button, continueButton;
    [SerializeField]
    TextMeshProUGUI decision1, decision2;
    [SerializeField]
    List<GameDecision> decisions;
    [SerializeField]
    Image enemy;
    GameDecision currentDecision;
    // Start is called before the first frame update
    void Start()
    {
        Load();
        currentDecision = decisions[Random.Range(0, decisions.Count)];
        StartCoroutine(DecisionScene());
    }

    private void Update()
    {
        foodUI.text = GameController.Instance.Food.ToString("0.0");
        fuelUI.text = GameController.Instance.Fuel.ToString("0.0");
        ammoUI.text = GameController.Instance.Ammo.ToString("0");
        teddyUI.text = GameController.Instance.Teddy.ToString("0");
        //teddy is unique, don't spoil it
        if (GameController.Instance.Teddy > 0)
        {
            teddyUI.transform.parent.gameObject.SetActive(true);
        }
    }

    private void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/run.dat"))
        {
            string data = File.ReadAllText(Application.persistentDataPath + "/run.dat");
            SaveData deserialized = JsonUtility.FromJson<SaveData>(data);
            GameController game = GameController.Instance;
            game.Ammo = deserialized.Ammo;
            game.Food = deserialized.Food;
            game.Fuel = deserialized.Fuel;
            game.Teddy = deserialized.Teddy;
        }
        else
        {
            GameController game = GameController.Instance;
        }
    }

    private void Save()
    {
        if (File.Exists(Application.persistentDataPath + "/run.dat"))
        {
            string data = File.ReadAllText(Application.persistentDataPath + "/run.dat");
            SaveData deserialized = JsonUtility.FromJson<SaveData>(data);
            GameController game = GameController.Instance;
            deserialized.Ammo = game.Ammo;
            deserialized.Food = game.Food;
            deserialized.Fuel = game.Fuel;
            deserialized.Teddy = game.Teddy;
            data = JsonUtility.ToJson(deserialized);
            File.WriteAllText(Application.persistentDataPath + "/run.dat", data);
        }
    }

    IEnumerator DecisionScene()
    {
        decisiondescription.gameObject.SetActive(true);
        decision1Button.gameObject.SetActive(false);
        decision2Button.gameObject.SetActive(false);
        string intro = currentDecision.intro;
        decisiondescription.text = "";
        while(decisiondescription.text != currentDecision.intro)
        {
            if (intro[0] != '<')
            {
                if (intro[0] == '"')
                    enemy.gameObject.SetActive(true);
                decisiondescription.text += intro[0];
                intro = intro.Substring(1);
            }
            else
            {
                while(intro[0] != '>')
                {
                    decisiondescription.text += intro[0];
                    intro = intro.Substring(1);
                }
            }
            yield return new WaitForSeconds(secondPerCharacterScroll);
        }
        decision1Button.gameObject.SetActive(true);
        decision2Button.gameObject.SetActive(true);
        //Hardcoded check to simulate certain choices being dependant on earlier actions
        if (currentDecision.choice1.teddycost > 0 && GameController.Instance.Teddy < 1)
            decision1Button.gameObject.SetActive(false);
        //Hardcoded check to simulate certain choices being dependant on earlier actions
        if (currentDecision.choice2.teddycost > 0 && GameController.Instance.Teddy < 1)
            decision2Button.gameObject.SetActive(false);
        decision1Button.interactable = currentDecision.choice1.allowPartialPayment || (GameController.Instance.Food >= currentDecision.choice1.foodcost && GameController.Instance.Fuel >= currentDecision.choice1.fuelcost && GameController.Instance.Ammo >= currentDecision.choice1.ammocost && GameController.Instance.Teddy >= currentDecision.choice1.teddycost);
        decision2Button.interactable = currentDecision.choice2.allowPartialPayment || (GameController.Instance.Food >= currentDecision.choice2.foodcost && GameController.Instance.Fuel >= currentDecision.choice2.fuelcost && GameController.Instance.Ammo >= currentDecision.choice2.ammocost && GameController.Instance.Teddy >= currentDecision.choice2.teddycost);
        decision1.text = currentDecision.choice1.decision;
        decision2.text = currentDecision.choice2.decision;
    }

    public void MakeChoice(int choice)
    {
        if (choice == 1)
            StartCoroutine(Choice(currentDecision.choice1));
        else
            StartCoroutine(Choice(currentDecision.choice2));
    }

    public void LoadMapScene()
    {
        Save();
        SceneManager.LoadScene("MapScene");
    }

    IEnumerator Choice(DecisionChoice choice)
    {
        GameController.Instance.Food = Mathf.Clamp(GameController.Instance.Food - choice.foodcost + choice.foodgain, 0, float.MaxValue);
        GameController.Instance.Fuel = Mathf.Clamp(GameController.Instance.Fuel - choice.fuelcost + choice.fuelgain, 0, float.MaxValue);
        GameController.Instance.Ammo = Mathf.Clamp(GameController.Instance.Ammo - choice.ammocost + choice.ammogain, 0, int.MaxValue);
        GameController.Instance.Teddy = Mathf.Clamp(GameController.Instance.Teddy - choice.teddycost + choice.teddygain, 0, int.MaxValue);
        decision1Button.gameObject.SetActive(false);
        decision2Button.gameObject.SetActive(false);
        decisiondescription.gameObject.SetActive(false);
        postdecisiondescription.gameObject.SetActive(true);
        string outro = choice.outro;
        postdecisiondescription.text = "";
        while (postdecisiondescription.text != choice.outro)
        {
            if (outro[0] != '<')
            {
                if (outro[0] == '"')
                    enemy.gameObject.SetActive(true);
                postdecisiondescription.text += outro[0];
                outro = outro.Substring(1);
            }
            else
            {
                while (outro[0] != '>')
                {
                    postdecisiondescription.text += outro[0];
                    outro = outro.Substring(1);
                }
                postdecisiondescription.text += outro[0];
                outro = outro.Substring(1);
            }
            yield return new WaitForSeconds(secondPerCharacterScroll);
        }
        continueButton.gameObject.SetActive(true);
    }
}
