using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public GameObject[] spawns = new GameObject[3];
    public GameObject enemy;

    private float timer;
    public float limit;
    public bool canSpawn;
    private int random;

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canSpawn) { 
            timer += Time.deltaTime;

            if (timer >= limit) {

            }
        }
    }

    public void spawnEnemy(string EnemyName, float healthMulti, float damageMulti, float speedMulti) {
        Instantiate(enemy, spawns[random].transform.position, Quaternion.identity);

        if (canSpawn && timer >= limit) {
            timer = 0;
            random = Random.Range(0, 3);
            

        }

    }
}
