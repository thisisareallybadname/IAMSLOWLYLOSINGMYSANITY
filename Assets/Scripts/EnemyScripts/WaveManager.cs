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
    public TMP_Text deathWaveReached;

    public float health = 5;

    HashSet<GameObject> enemies = new HashSet<GameObject>();
    public GameObject[] spawns = new GameObject[3];
    private float randomSpawn;

    public TMP_Text enemyCounter;
    public TMP_Text waveCounter;

    private PlayerDamage playerHealth;

    // Start is called before the first frame update
    void Start() {
        playerHealth = GameObject.Find("Player").GetComponent<PlayerDamage>();

    }

    // Update is called once per frame
    void FixedUpdate() {

        if (!intermission) {
            enemyCounter.text = "Enemies Left: " + enemies.Count;
        
        } else {
            if (Mathf.Ceil(waveDelay - waveCooldown) >= (waveDelay * 0.8) && wave > 0) {
                enemyCounter.text = "Wave Completed";

            } else {
                enemyCounter.text = "Next wave in " + Mathf.Ceil(waveDelay - waveCooldown);

            }

        }

        waveCounter.text = "Wave " + wave;
        deathWaveReached.text = "Waves Survived: " + wave;

        if (timer >= spawnDelay && enemiesSpawned < enemiesPerWave && spawningEnemies) {
            timer = 0;
            randomSpawn = UnityEngine.Random.Range(0, spawns.Length);

            GameObject newEnemy = Instantiate(enemy, spawns[((int)randomSpawn)].transform.position, Quaternion.identity);
            newEnemy.GetComponent<enemyAI>().chasePlayer = true;
            enemies.Add(newEnemy);
            
            enemiesSpawned++;

            // if wave over, add multipliers to enemies
            if (enemiesSpawned >= enemiesPerWave) {
                spawningEnemies = false;
                enemiesPerWave += wave;
                enemy.GetComponent<EnemyHealth>().setHealth(health + wave);
                
            }
        }

        if (timer < spawnDelay) {
            timer += Time.fixedDeltaTime;
 
        }

        //intermission period

        if (enemies.Count == 0) {
            if (waveCooldown < waveDelay && !spawningEnemies) {
                waveCooldown += Time.fixedDeltaTime;

                intermission = true;
                playerHealth.setHealth(5);

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
}
