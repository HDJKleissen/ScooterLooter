using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenSpriteSpawner : MonoBehaviour
{
    public Sprite[] spriteImages;
    public GameObject spritePrefab;

    public int SpritesAmount;
    public float spriteZMin, spriteZMax;

    Rotate[] sprites;
    // Start is called before the first frame update
    void Start()
    {
        sprites = new Rotate[SpritesAmount];
        SpawnSprites();
    }

    void SpawnSprites()
    {
        for (int i = 0; i < SpritesAmount; i++)
        {
            sprites[i] = Instantiate(spritePrefab).GetComponent<Rotate>();
            sprites[i].GetComponent<SpriteRenderer>().sprite = spriteImages[Random.Range(0, spriteImages.Length)];
            
            ResetSpriteThrow(i);
        }
    }

    void ResetSpriteThrow(int spriteNum)
    {
        sprites[spriteNum].transform.position = GetRandomPosAtBottom();
        sprites[spriteNum].GetComponent<Rigidbody>().velocity = GetRandomVelocity();
        sprites[spriteNum].RotateVector = GetRandomRotation();
    }

    Vector3 GetRandomPosAtBottom()
    {
        Vector3 returnVector = Camera.main.ViewportToWorldPoint(new Vector3(Random.value, 0, 10));
        returnVector.y = Random.Range(-20f, -30f);
        returnVector.z = Random.Range(spriteZMin, spriteZMax);

        return returnVector;
    }

    Vector3 GetRandomVelocity()
    {
        return new Vector3(Random.Range(-5f, 5f), Random.Range(15f, 45f), Random.Range(-5f, 5f));
    }
    Vector3 GetRandomRotation()
    {
        return new Vector3(Random.Range(-45f, 45f), Random.Range(-45f, 45f), Random.Range(-45f, 45f));
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < SpritesAmount; i++)
        {
            if(sprites[i].transform.position.y < -30)
            {
                ResetSpriteThrow(i);
            }
        }
    }
}
