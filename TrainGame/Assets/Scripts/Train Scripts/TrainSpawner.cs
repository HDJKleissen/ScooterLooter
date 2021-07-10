using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainSpawner : MonoBehaviour
{
    public int AmountOfCarts;
    public GameObject Locomotive;
    public GameObject CartToSpawn;
    public float SpawnInterval;
    public float TrainSpeed;
    public Waypoint CartGoal;

    private float timeToNextCart = 0;
    private bool spawnedLocomotive;
    private float cartSizeHalved;

    private float cartDistance;

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
        if(cartDistance == 0){
            var locomotiveRenderer = Locomotive.GetComponent<SpriteRenderer>();
            var locomotiveHalfSize = locomotiveRenderer.bounds.size.x/2;
            var cartRenderer = CartToSpawn.GetComponent<SpriteRenderer>();
            cartSizeHalved = cartRenderer.bounds.size.x/4;
            cartDistance = cartSizeHalved + locomotiveHalfSize;
            SpawnInterval = cartDistance / TrainSpeed;
        
            cartDistance = cartSizeHalved * 2;
        }
        else
        {
            SpawnInterval = cartDistance *2 /TrainSpeed;
        }
    }
}
