using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    static EnemySpawner Instance;
    static List<EnemySpawner> spawners;
    [SerializeField]
    GameObject Prefab;
    [SerializeField]
    float secondsPerEnemy = 4f;
    bool spawning = true;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            spawners = new List<EnemySpawner>();
            spawners.Add(this);
        }
        else
            spawners.Add(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        if(Instance == this)
            StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        yield return new WaitUntil(() => LooterGameController.Instance.timerStarted);
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1, LooterGameController.Instance.time <= 0 ? 1 : secondsPerEnemy));
            List<EnemySpawner> viableSpawners=spawners.FindAll((x) => x.spawning);
            EnemySpawner spawner = viableSpawners[Random.Range(0, viableSpawners.Count)];
            Instantiate(Prefab, spawner.transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != null && collision.gameObject.CompareTag("NoSpawnZone"))
            spawning = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject != null && collision.gameObject.CompareTag("NoSpawnZone"))
            spawning = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 1);
    }
}
