using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class WaveManager : MonoBehaviour {

    private float timer;
    public float waveDelay;
    public float spawnDelay;

    public float enemiesPerWave;

    public EnemySpawnManager spawner;
    public GameObject enemy;

    HashSet<GameObject> enemies = new HashSet<GameObject>();
    public GameObject[] spawns = new GameObject[3];
    private float randomSpawn;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void FixedUpdate() {

        if (timer > waveDelay) {
            StartCoroutine(spawnWave());
            timer = -123123;
        }

        timer += Time.fixedDeltaTime;
    }

    private IEnumerator spawnWave() {

        randomSpawn = UnityEngine.Random.Range(0, 3);
        for (int i = 0; i < enemiesPerWave; i++) {
            //spawner.spawnEnemy("a", 1, 1, 1);
            Instantiate(enemy, spawns[((int)randomSpawn)].transform.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnDelay);

        }
        
        //Instantiate(enemy, spawns[(int)randomSpawn].transform.position, Quaternion.identity);
        enemies.Add(null);

        yield return null;
    }
}
