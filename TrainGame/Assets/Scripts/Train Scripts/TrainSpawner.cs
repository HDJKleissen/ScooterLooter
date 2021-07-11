using UnityEngine;

public class TrainSpawner : MonoBehaviour
{
    private Vector3 TrainOriginPosition;
    public int AmountOfCarts;
    public GameObject Locomotive;
    public GameObject CartToSpawn;

    public TrainController TrainControllerScript;
    public GameObject SpriteParent;

    private bool spawnedLocomotive;
    private float nextCartSizeHalved;

    private float spawnDistance;
    private int cartsRemaining;
    private GameObject lastCreatedCart;

    void Start()
    {
        cartsRemaining = AmountOfCarts;
        TrainOriginPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(lastCreatedCart == null || 
        (Mathf.Abs(TrainOriginPosition.x - lastCreatedCart.transform.position.x) >= spawnDistance && cartsRemaining > 0))
        {
            SpawnCart();
            cartsRemaining -= 1;
        }
        CalculateDistanceToNextCar();
    }

    void SpawnCart()
    {
        var cartPosition = TrainOriginPosition;
        if (lastCreatedCart != null)
        {
            var lastCartPosition = lastCreatedCart.transform.position;
            lastCartPosition.x += spawnDistance;
            cartPosition = lastCartPosition;
        }
        lastCreatedCart = Instantiate(GetCurrentCartToSpawn(), cartPosition, transform.rotation, SpriteParent.transform);
    }

    //To be filled at a later date with code to determine what to spawn
    GameObject GetCurrentCartToSpawn()
    {
        if (!spawnedLocomotive)
        {
            spawnedLocomotive = true;
            return Locomotive;
        }
        return CartToSpawn;
    }

    void CalculateDistanceToNextCar()
    {
        var trainSpeed = TrainControllerScript.MoveSpeed;
        if (trainSpeed > 0)
        {
            var previousCartRenderer = lastCreatedCart.GetComponent<SpriteRenderer>();
            var previousCartHalfSize = previousCartRenderer.bounds.size.x / 2;
            var nextCartRenderer = CartToSpawn.GetComponent<SpriteRenderer>();
            nextCartSizeHalved = nextCartRenderer.bounds.size.x / 2;
            spawnDistance = nextCartSizeHalved + previousCartHalfSize;
        }
    }
}
