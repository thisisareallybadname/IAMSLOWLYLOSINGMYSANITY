using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour {

    private float timer;
    public float waveDelay;
    public float spawnDelay;

    private float countdown;

    public float enemiesPerWave;
    private float enemiesSpawned;

    public EnemySpawnManager spawner;
    public GameObject enemy;

    HashSet<GameObject> enemies = new HashSet<GameObject>();
    public GameObject[] spawns = new GameObject[3];
    private float randomSpawn;

    public TMP_Text enemyCounter;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void FixedUpdate() {
        enemyCounter.text = "Enemies Left: " + enemies.Count;
        if (countdown < waveDelay || enemies.Count == 0) {
            StartCoroutine(spawnWave());
            enemiesSpawned = 0;

        }

        if (timer >= spawnDelay && enemiesSpawned < enemiesPerWave) {
            GameObject newEnemy = Instantiate(enemy, spawns[((int)randomSpawn)].transform.position, Quaternion.identity);
            newEnemy.GetComponent<enemyAI>().chasePlayer = true;
            enemies.Add(newEnemy);
            enemiesSpawned++;
        }

        if (timer < spawnDelay) {
            timer += Time.fixedDeltaTime;
 
        }
        if (countdown < waveDelay) {
            countdown += Time.fixedDeltaTime;
        }
    }

    private IEnumerator spawnWave() {

        randomSpawn = UnityEngine.Random.Range(0, 3);

        yield return null;
    }

    public void enemyDeath(GameObject enemy) {
        enemies.Remove(enemy);

    }
}
