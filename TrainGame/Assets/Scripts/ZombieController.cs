using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : NPCController
{
    public Sprite[] spriteChoices;

    SpriteRenderer spriteRenderer;
    BoxCollider2D trainCollider;


    bool changeIdleDirection = true;

    bool attackingTrain = false;
    bool chooseTrainAttackPosition = true;
    Vector3 trainAttackPosition;

    ZombieBehaviours currentBehaviour = ZombieBehaviours.IDLE;
    // Start is called before the first frame update
    void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        spriteRenderer.sprite = spriteChoices[Random.Range(0, spriteChoices.Length)];
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Train")
        {
            attackingTrain = true;
        }
    }

    // ew
    void ResetTrainBools()
    {
        chooseTrainAttackPosition = true;
        attackingTrain = false;
    }

    protected override Vector3 GetMovement()
    {
        Vector3 playerPos = GameController.Instance.Player.transform.position;
        if (trainCollider == null)
        {
            trainCollider = GameController.Instance.Train.GetComponent<BoxCollider2D>();
        }

        playerPos.z = 0;
        currentBehaviour = ZombieDirector.GetBehaviour(this);
        switch (currentBehaviour)
        {
            case ZombieBehaviours.STAND_STILL:
                ResetTrainBools();
                return Vector3.zero;
            case ZombieBehaviours.IDLE:
                ResetTrainBools();
                if (changeIdleDirection)
                {
                    changeIdleDirection = false;
                    StartCoroutine(CoroutineHelper.DelaySeconds(() => changeIdleDirection = true, Random.Range(0.5f, 2.5f)));

                    bool returnStandingStill = Random.Range(0, 1f) > 0.5f;
                    if (returnStandingStill)
                    {
                        return Vector3.zero;
                    }
                    else
                    {
                        return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                    }
                }
                else
                {
                    return movement;
                }
            case ZombieBehaviours.ATTACK_TRAIN:
                if (chooseTrainAttackPosition)
                {
                    trainAttackPosition = new Vector3(Random.Range(trainCollider.bounds.min.x, trainCollider.bounds.max.x), Random.Range(trainCollider.bounds.min.y, trainCollider.bounds.max.y));
                }
                if (attackingTrain)
                {
                    return Vector3.zero;
                }
                return (trainAttackPosition - transform.position).normalized;
            case ZombieBehaviours.FOLLOW_PLAYER:
                ResetTrainBools();
                Debug.DrawRay(transform.position, playerPos - transform.position, Color.green);
                return (playerPos - transform.position).normalized;

            default:
                Debug.LogError("Zombie Behaviour " + currentBehaviour + " not implemented");
                return Vector3.zero;
        }
    }

    protected override void DoAttacking()
    {
        if (attackingTrain)
        {
            attackTimer += Time.deltaTime;

            if(attackTimer > AttackSpeed)
            {
                attackTimer = 0;
                GameController.Instance.Train.DealDamage(AttackDamage);
            }
        }
        
    }
}
