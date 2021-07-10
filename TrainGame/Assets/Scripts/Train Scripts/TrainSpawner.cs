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

    private float spawnDistance;
    private int cartsRemaining;
    private GameObject lastCreatedCart;

    void Start()
    {
        cartsRemaining = AmountOfCarts;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeToNextCart = timeToNextCart - Time.deltaTime;
        if(timeToNextCart <= 0 && cartsRemaining > 0)
        {
            SpawnCart();
            cartsRemaining -= 1;
        }
    }

    void SpawnCart(){
        var cartPosition = transform.position;
        if(lastCreatedCart != null){
            var lastCartPosition = lastCreatedCart.transform.position;
            lastCartPosition.x += spawnDistance;
            cartPosition = lastCartPosition;
        }
        lastCreatedCart = Instantiate(GetCurrentCartToSpawn(),cartPosition,transform.rotation);
        var cartMovement = lastCreatedCart.GetComponent<CartMovement>();
        cartMovement.CurrentGoal = CartGoal;
        cartMovement.SpeedPerSec = TrainSpeed;
        timeToNextCart = SpawnInterval;
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
        if(spawnDistance == 0)
        {
            var locomotiveRenderer = Locomotive.GetComponent<SpriteRenderer>();
            var locomotiveHalfSize = locomotiveRenderer.bounds.size.x/2;
            var cartRenderer = CartToSpawn.GetComponent<SpriteRenderer>();
            cartSizeHalved = cartRenderer.bounds.size.x/2;
            spawnDistance = cartSizeHalved + locomotiveHalfSize;
            SpawnInterval = spawnDistance / TrainSpeed;
        }
        else
        {
            var cartRenderer = CartToSpawn.GetComponent<SpriteRenderer>();
            spawnDistance = cartRenderer.bounds.size.x;
            SpawnInterval = spawnDistance /TrainSpeed;
        }
    }
}
