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
    
    private float waveCooldown;

    public float enemiesPerWave;
    private float enemiesSpawned;

    private float enemiesKilled = 0;

    private bool spawningEnemies = false;
    private bool intermission;

    public EnemySpawnManager spawner;
    public GameObject enemy;

    public float wave;
    public TMP_Text waveCounter;

    HashSet<GameObject> enemies = new HashSet<GameObject>();
    public GameObject[] spawns = new GameObject[3];
    private float randomSpawn;

    public PlayerDamage playerHealth;

    public TMP_Text enemyCounter;
    public TMP_Text waveReached;

    public bool startGame;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void FixedUpdate() {

        if (!intermission) {
            enemyCounter.text = "Enemies Left: " + enemies.Count;
        
        } else {
            if (Mathf.Ceil(waveDelay - waveCooldown) > waveDelay * 0.8f && wave > 0) {
                enemyCounter.text = "Wave Complete";

            } else {
                enemyCounter.text = "Next wave in " + Mathf.Ceil(waveDelay - waveCooldown);
            }

        }

        waveCounter.text = "Wave " + wave;
        waveReached.text = "Reached Wave " + wave;

        if (timer >= spawnDelay && enemiesSpawned < enemiesPerWave && spawningEnemies) {
            timer = 0;
            randomSpawn = UnityEngine.Random.Range(0, 3);

            GameObject newEnemy = Instantiate(enemy, spawns[((int)randomSpawn)].transform.position, Quaternion.identity);
            newEnemy.GetComponent<enemyAI>().chasePlayer = true;
            enemies.Add(newEnemy);
            
            enemiesSpawned++;

            if (enemiesSpawned >= enemiesPerWave) {
                spawningEnemies = false;
                enemiesPerWave = Mathf.Ceil(wave * 1.5f);
                enemy.GetComponent<Enemy>().addStatAmplifier(enemiesPerWave + Mathf.Ceil(wave * 0.125f));
            }
        }

        if (timer < spawnDelay) {
            timer += Time.fixedDeltaTime;
 
        }

        //intermission period

        if (enemies.Count == 0) {
            if (waveCooldown < waveDelay && !spawningEnemies) {
                waveCooldown += Time.fixedDeltaTime;
                playerHealth.setHealth(5);
                intermission = true;

            } else {
                if (!spawningEnemies) {
                    wave++;
                    intermission = false;
                    waveCooldown = 0;
                    enemiesSpawned = 0;
                    spawningEnemies = true;
                }

            }
        }
    }

    public void enemyDeath(GameObject enemy) {
        enemies.Remove(enemy);
        enemiesKilled++;
    }

    public void StartWave() {


    }
}
