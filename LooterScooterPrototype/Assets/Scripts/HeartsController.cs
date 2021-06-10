using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartsController : MonoBehaviour
{
    public List<SpriteRenderer> hearts;

    public void ShowHearts(int amount, bool blinkLastOff = true)
    {
        if (blinkLastOff)
            StartCoroutine(ShowOff(amount));
        else
            StartCoroutine(ShowOn(amount));
    }

    IEnumerator ShowOff(int amount)
    {
        for(int i = 0; i < amount + 1; i++)
        {
            hearts[i].enabled = true;
        }
        yield return new WaitForSeconds(0.25f);
        hearts[amount].enabled = false;
        yield return new WaitForSeconds(0.25f);
        hearts[amount].enabled = true;
        yield return new WaitForSeconds(0.25f);
        hearts[amount].enabled = false;
        yield return new WaitForSeconds(0.25f);
        
        for (int i = 0; i < amount + 1; i++)
        {
            hearts[i].enabled = false;
        }
    }

    IEnumerator ShowOn(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            hearts[i].enabled = true;
        }
        yield return new WaitForSeconds(0.25f);
        hearts[amount - 1].enabled = false;
        yield return new WaitForSeconds(0.25f);
        hearts[amount - 1].enabled = true;
        yield return new WaitForSeconds(0.25f);
        hearts[amount - 1].enabled = false;
        yield return new WaitForSeconds(0.25f);
        hearts[amount - 1].enabled = true;
        yield return new WaitForSeconds(0.25f);

        for (int i = 0; i < amount; i++)
        {
            hearts[i].enabled = false;
        }
    }
}
