using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [SerializeField]
    LooterScooterControlScheme controlScheme;
    [SerializeField]
    Transform bulletOrigin;
    [SerializeField]
    Transform heldObjectTransform;
    [SerializeField]
    Material shotTraceMaterial;
    [SerializeField]
    LayerMask shootingMask;

    AudioSource source;
    [SerializeField]
    AudioClip chainsaw;
    [SerializeField]
    AudioClip reload;
    [SerializeField]
    AudioClip gunshot;
    [SerializeField]
    AudioClip click;
    
    new Rigidbody2D rigidbody;
    SpriteRenderer renderer;
    Animator animator;
    GameObject heldObject;

    public SpriteRenderer leftClick;
    public SpriteRenderer rightClick;

    bool shotonce = false;
    bool sawedonce = false;
    bool pickeduponce = false;
    bool thrownonce = false;

    bool reloading = false;
    bool sawing = false;
    bool flipHorizontal = false;
    public bool alive = true;

    public HeartsController HeartsObject;
    public int hearts = 3;
    bool immune = false;
    float timeSinceDamage = 0;

    const float speed = 7.5f;
    const float reloadSpeed = 1f;
    const float shotTraceLength = 25;
    const float shotTraceTime = .1f;
    const float sawingTime = 1;
    const float immunityTime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;

        rigidbody = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();

        StartCoroutine(Tutorial());
    }

    // Update is called once per frame
    void Update()
    {
        if (!alive)
            return;
        timeSinceDamage += Time.deltaTime;
        if(timeSinceDamage > 5)
        {
            timeSinceDamage = 0;
            if (hearts < 3)
            {
                hearts += 1;
                HeartsObject.ShowHearts(hearts, false);
            }
        }
        //I don't know why this is necessary, but it's late and I'm tired
        if (heldObject != null)
            heldObject.transform.position = heldObjectTransform.position;

        //Calculate the direction of the input
        var direction = Input.mousePosition - Camera.main.WorldToScreenPoint(bulletOrigin.position);

        //Handle Shooting
        if (Input.GetKeyDown(controlScheme.Shoot))
        {
            //If shooting while holding an object, throw the object
            if (heldObject != null)
            {
                thrownonce = true;
                BoxCollider2D heldCollider = heldObject.GetComponent<BoxCollider2D>();
                heldCollider.enabled = true;
                Rigidbody2D heldRigidbody = heldObject.GetComponent<Rigidbody2D>();
                heldRigidbody.velocity = direction.normalized * 40;
                heldRigidbody.drag = 10;
                heldObject = null;
            }
            //If not holding an object, shoot the gun
            else if(!reloading && GameController.Instance.Ammo > 0)
            {
                GameController.Instance.Ammo -= 1;
                shotonce = true;
                reloading = true;
                source.clip = gunshot;
                source.pitch = Random.Range(0.9f, 1.1f);
                source.Play();
                StartCoroutine(ExecuteLambdaAfterTime(gunshot.length, () =>
                {
                    source.clip = reload;
                    source.pitch = Random.Range(0.9f, 1.1f);
                    source.Play();
                }));
                StartCoroutine(ExecuteLambdaAfterTime(reloadSpeed, () => { reloading = false; }));
                //Raycast to see if the bullet hits a collider
                var hit = Physics2D.Raycast(bulletOrigin.position, direction, shotTraceLength, shootingMask.value);
                //Create a shot trace
                LineRenderer line = new GameObject("Shot Trace").AddComponent<LineRenderer>();
                line.sortingOrder = 2000;
                line.material = shotTraceMaterial;
                line.startWidth = 0.05f;
                line.endWidth = 0.05f;
                line.SetPosition(0, bulletOrigin.position - new Vector3(0, 0, .04f));
                line.SetPosition(1, -new Vector3(0, 0, .04f) + (hit.collider != null ? (Vector3)hit.point : (bulletOrigin.position + (direction.normalized * shotTraceLength))));
                //Color the shot trace
                line.startColor = Color.white;
                line.endColor = Color.white;
                //Delete the shot trace after a second
                Destroy(line.gameObject, shotTraceTime);

                //Check if the shot hit an enemy
                if (hit.collider != null)
                {
                    var enemy = hit.collider.gameObject.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.Shot(direction.normalized);
                    }
                }
            }
        }

        //Handle Interact button
        if (Input.GetKeyDown(controlScheme.PickUp))
        {
            //Check for pickups
            Collider2D[] hits = Physics2D.OverlapBoxAll(new Vector2(transform.position.x, transform.position.y - 1), new Vector2(2, 2), 0);
            foreach (Collider2D hit in hits)
            {
                if (hit.gameObject.CompareTag("Pickup"))
                {
                    pickeduponce = true;
                    PickUpObject(hit.gameObject);
                }
            }
        }

        //Handle Movement Input
        float horizontal = (Input.GetKey(controlScheme.Right) ? 1 : 0) - (Input.GetKey(controlScheme.Left) ? 1 : 0);
        float vertical = (Input.GetKey(controlScheme.Up) ? 1 : 0) - (Input.GetKey(controlScheme.Down) ? 1 : 0);

        rigidbody.velocity = new Vector3(horizontal, vertical).normalized * speed;

        //Update animator and renderer variables
        if (direction.x < 0)
            flipHorizontal = true;
        else if (direction.x > 0)
            flipHorizontal = false;

        renderer.flipX = flipHorizontal;
        animator.SetFloat("VelocityX", Mathf.Abs(horizontal));
        animator.SetBool("Holding", heldObject != null);
    }

    void PickUpObject(GameObject pickup)
    {
        if (heldObject != null)
            return;
        heldObject = pickup;
        var heldCollider = heldObject.GetComponent<BoxCollider2D>();
        heldCollider.enabled = false;
    }

    IEnumerator ExecuteLambdaAfterTime(float time, Action lambda)
    {
        yield return new WaitForSeconds(time);
        lambda.Invoke();
    }

    IEnumerator Tutorial()
    {
        if (PlayerPrefs.GetInt("Tutorial", 0) != 0)
            yield break;
        leftClick.enabled = true;
        yield return new WaitUntil(() => shotonce);
        leftClick.enabled = false;
        while(!pickeduponce)
        {
            bool hasHit = false;
            //Check for pickups
            Collider2D[] hits = Physics2D.OverlapBoxAll(new Vector2(transform.position.x, transform.position.y - 1), new Vector2(2, 2), 0);
            foreach (Collider2D hit in hits)
            {
                if (hit.gameObject.CompareTag("Pickup"))
                {
                    hasHit = true;
                }
            }
            rightClick.enabled = hasHit;
            yield return null;
        }
        rightClick.enabled = false;
        leftClick.enabled = true;
        yield return new WaitUntil(() => thrownonce);
        leftClick.enabled = false;

        Destroy(leftClick.gameObject);
        Destroy(rightClick.gameObject);
        PlayerPrefs.SetInt("Tutorial", 1);
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(3);
        File.Delete(Application.persistentDataPath + "/run.dat");
        SceneManager.LoadScene("Menu");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Spider")
        {
            if (!immune)
            {
                timeSinceDamage = 0;
                hearts -= 1;
                HeartsObject.ShowHearts(hearts);
                immune = true;
                if(hearts > 0)
                    StartCoroutine(ExecuteLambdaAfterTime(immunityTime, () => { immune = false; }));
                else
                {
                    alive = false;
                    rigidbody.velocity = Vector3.zero;
                    animator.SetBool("Alive", false);
                    GetComponent<Collider2D>().enabled = false;
                    StartCoroutine(Die());
                }
            }
        }
    }
}

[Serializable]
public struct LooterScooterControlScheme
{
    public KeyCode Up, Down, Left, Right, Shoot, PickUp;
}
