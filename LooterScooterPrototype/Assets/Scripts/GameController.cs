using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    
    public static GameController Instance {
        get {
            if(instance == null)
            {
                instance = new GameObject("GameController").AddComponent<GameController>();
                
            }
            return instance;
        }
    }
    public int Ammo = 10, Teddy = 0;
    public float Fuel = 2, Food = 5;

    static GameController instance;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
