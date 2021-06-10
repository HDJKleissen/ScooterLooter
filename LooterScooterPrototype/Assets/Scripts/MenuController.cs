using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    Button ContinueButton;
    // Start is called before the first frame update
    void Start()
    {
        ContinueButton.gameObject.SetActive(File.Exists(Application.persistentDataPath + "/run.dat"));
    }

    public void StartNewGame()
    {
        File.Delete(Application.persistentDataPath + "/run.dat");
        SceneManager.LoadScene("MapScene");
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene("MapScene");
    }
}
