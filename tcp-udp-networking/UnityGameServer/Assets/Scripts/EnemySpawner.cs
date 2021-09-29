using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float frequency = 3f;
    public bool max_reached = false;
    int all_killed = 0;

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(frequency);

        if (max_reached == false && Enemy.enemies.Count == Enemy.maxEnemies)
        {
            max_reached = true;
        }
        if (max_reached && Enemy.enemies.Count == 0)
        {
            all_killed++;
        }
        if (all_killed < 2 && NetworkManager.instance.stop == false)
        {
            if (Enemy.enemies.Count < Enemy.maxEnemies)
            {
                Debug.Log("enemies count: " + Enemy.enemies.Count);
                NetworkManager.instance.InstantiateEnemy(transform.position);
            }
            StartCoroutine(SpawnEnemy());
        }
        else
        {
            ServerSend.GameEnd(all_killed);
            NetworkManager.instance.stop = true;
        }

    }
}
