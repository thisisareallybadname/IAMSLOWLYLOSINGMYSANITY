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

    public bool spawningEnemies = false;
    public bool intermission;

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
    public bool waveEnded;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (startGame) {
            Playing();

        }
    }

    public void enemyDeath(GameObject enemy) {
        enemies.Remove(enemy);
        enemiesKilled++;
    }

    public float EnemiesLeft() {
        return enemies.Count;

    }

    private void Playing() {
        if (!intermission)
        {
            enemyCounter.text = "Enemies Left: " + enemies.Count;

        }
        else
        {
            if (Mathf.Ceil(waveDelay - waveCooldown) > waveDelay * 0.8f && wave > 0)
            {
                enemyCounter.text = "Wave Complete";

            }
            else
            {
                enemyCounter.text = "Next wave in " + Mathf.Ceil(waveDelay - waveCooldown);
            }

        }

        waveCounter.text = "Wave " + wave;
        waveReached.text = "Reached Wave " + wave;

        if (timer >= spawnDelay && enemiesSpawned < enemiesPerWave && spawningEnemies)
        {
            timer = 0;
            randomSpawn = UnityEngine.Random.Range(0, 3);

            GameObject newEnemy = Instantiate(enemy, spawns[((int)randomSpawn)].transform.position, Quaternion.identity);
            newEnemy.GetComponent<Enemy>().active = true;
            enemies.Add(newEnemy);
            enemy.GetComponent<EnemyHealth>().addStatAmplifier(wave);

            enemiesSpawned++;

            if (enemiesSpawned >= enemiesPerWave)
            {
                spawningEnemies = false;
            }
        }

        if (timer < spawnDelay)
        {
            timer += Time.fixedDeltaTime;

        }

        //intermission period

        if (enemies.Count == 0)
        {
            if (waveCooldown < waveDelay && !spawningEnemies)
            {
                waveCooldown += Time.fixedDeltaTime;
                playerHealth.setHealth(5);
                intermission = true;
                waveEnded = true;

            }
            else
            {
                if (!spawningEnemies)
                {
                    waveEnded = false;
                    wave++;
                    intermission = false;
                    waveCooldown = 0;
                    enemiesSpawned = 0;

                    enemiesPerWave = 5 + wave;

                    spawningEnemies = true;
                }

            }
        }

    }
}
