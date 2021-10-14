using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public GameObject enemy, enemySpawn;
    GameObject[] enemyPool;
    //int enemyPoolCount = 0;
    void Start()
    {
        EnemyPool();
        StartCoroutine(EnemyPoolActive());
    }


    void EnemyPool()
    {
        enemyPool = new GameObject[10];
        for (int i = 0; i < enemyPool.Length; i++)
        {
            enemyPool[i] = Instantiate(enemy);
            enemyPool[i].SetActive(false);
        }
    }
    IEnumerator EnemyPoolActive()
    {
        for (int i = 0; i < enemyPool.Length; i++)
        {
            yield return new WaitForSeconds(3);
            enemyPool[i].SetActive(true);
            enemyPool[i].transform.position = enemySpawn.transform.position;
        }
    }
    /*void FixedUpdate()
    {
        StartCoroutine(EnemyPoolActive());
    }*/
}
