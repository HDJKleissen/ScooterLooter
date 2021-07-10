using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainSpawner : MonoBehaviour
{
    public int AmountOfCarts;
    public GameObject Locomotive;
    public GameObject CartToSpawn;
    public float SpawnInterval;
    public int TrainSpeed;
    public Waypoint CartGoal;

    private float timeToNextCart = 0;
    private bool spawnedLocomotive;
    private float cartSizeHalved;

    public float cartDistance;

    // Update is called once per frame
    void Update()
    {
        timeToNextCart = timeToNextCart - Time.deltaTime;
        if(timeToNextCart <= 0)
        {
            timeToNextCart = SpawnInterval;
            var newCart = Instantiate(GetCurrentCartToSpawn(),transform.position,transform.rotation);
            var cartMovement = newCart.GetComponent<CartMovement>();
            cartMovement.CurrentGoal = CartGoal;
            cartMovement.SpeedPerSec = TrainSpeed;
        }
    }

    //To be filled at a later date with code to determine what to spawn
    GameObject GetCurrentCartToSpawn()
    {
        
        CalculateTimeToNextCar();
        if(!spawnedLocomotive)
        {
            spawnedLocomotive = true;
            return Locomotive;
        }
        return CartToSpawn;
    }

    void CalculateTimeToNextCar()
    {
        if(!spawnedLocomotive){
        var locomotiveRenderer = Locomotive.GetComponent<SpriteRenderer>();
        var locomotiveHalfSize = locomotiveRenderer.size.x/2;
        var cartRenderer = CartToSpawn.GetComponent<SpriteRenderer>();
        cartSizeHalved = cartRenderer.size.x/2;
        cartDistance = cartSizeHalved + locomotiveHalfSize;
        }
    }
}
