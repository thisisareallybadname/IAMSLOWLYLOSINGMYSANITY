using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject[] spawns = new GameObject[3];
    public GameObject enemy;

    private float timer;
    private bool canSpawn;
    private int random;

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.deltaTime;

        if (timer >= 0.25f) {
            timer = 0;

            random = Random.Range(0, 3);
            Instantiate(enemy, spawns[random].transform.position, Quaternion.identity);

        }
    }
}
