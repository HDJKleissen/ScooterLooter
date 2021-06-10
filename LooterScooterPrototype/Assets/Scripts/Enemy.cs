using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    bool alive = true;
    EnemyFSM state = EnemyFSM.Marching;
    Transform player, target;
    new Rigidbody2D rigidbody;
    Animator animator;
    SpriteRenderer renderer;
    AudioSource source;
    public List<AudioClip> rear;
    public AudioClip die;

    const float speed = 5;
    const float jumpspeed = 15;
    const float jumpingDistance = 6.5f;
    const float playerDetectionDistance = 7f;
    const float rearTime = 0.8f;
    const float jumpTime = 0.4f;
    const float fadeTime = .75f;
    bool subtractFromSpiderCount = false;

    EnemyFSM State {
        get { return state; }
        set {
            switch (value)
            {
                case EnemyFSM.Rearing:
                    animator.SetBool("Rearing", true);
                    if (target == player)
                    {
                        source.volume = 0.2f;
                        source.clip = rear[Random.Range(0, rear.Count)];
                        source.pitch = Random.Range(0.9f, 1.1f);
                        source.Play();
                    }
                    rigidbody.velocity = Vector3.zero;
                    break;
                case EnemyFSM.Marching:
                    animator.SetBool("Rearing", false);
                    rigidbody.velocity = Vector3.zero;
                    break;
                case EnemyFSM.Jumping:
                    rigidbody.drag = 0;
                    rigidbody.velocity = (target.position - transform.position).normalized * jumpspeed;
                    break;
            }
            state = value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        source = GetComponent<AudioSource>();

        player = GameObject.Find("Player").transform;
        target = player;
        StartCoroutine(FSM());
    }

    // Update is called once per frame
    void Update()
    {
        if (rigidbody.velocity.x < 0)
            renderer.flipX = true;
        if (rigidbody.velocity.x > 0)
            renderer.flipX = false;
    }

    public void Shot(Vector3 direction)
    {
        StopAllCoroutines();
        State = EnemyFSM.Dead;
        GetComponent<Collider2D>().enabled = false;
        alive = false;
        source.volume = 1;
        source.clip = die;
        source.pitch = Random.Range(0.9f, 1.1f);
        source.Play();
        rigidbody.velocity = direction * 40;
        rigidbody.drag = 10;

        StartCoroutine(FadeOut(fadeTime));
        Destroy(gameObject, fadeTime);
    }

    IEnumerator FadeOut(float time)
    {
        Renderer renderer = GetComponent<Renderer>();
        var color = renderer.material.color;
        float opacity = 1;
        while (opacity > 0)
        {
            opacity -= Time.deltaTime / time;
            renderer.material.color = new Color(color.r, color.g, color.b, opacity);
            yield return null;
        }
    }

    IEnumerator FSM()
    {
        while (alive) {
            switch (State)
            {
                case EnemyFSM.Marching:
                    float playerdistance = (target.position - transform.position).magnitude;
                    if (playerdistance < playerDetectionDistance)
                    {
                        State = EnemyFSM.Rearing;
                        break;
                    }
                    //If the enemy doesn't jump, it marches towards it's target
                    rigidbody.velocity = (target.position - transform.position).normalized * speed;
                    yield return null;
                    break;
                case EnemyFSM.Rearing:
                    rigidbody.velocity = Vector3.zero;
                    yield return new WaitForSeconds(rearTime);
                    State = EnemyFSM.Jumping;
                    break;
                case EnemyFSM.Jumping:
                    yield return new WaitForSeconds(jumpTime);
                    State = EnemyFSM.Marching;
                    break;
                default:
                    rigidbody.velocity = Vector3.zero;
                    break;
            }
            yield return null;
        }
    }
}

public enum EnemyFSM { Marching, Rearing, Jumping, Dead}
