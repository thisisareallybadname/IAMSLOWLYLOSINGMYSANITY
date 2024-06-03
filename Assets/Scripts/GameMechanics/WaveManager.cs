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

    public GameObject enemy;
    private Enemy enemyProperties;

    public float wave = 0;
    public TMP_Text waveCounter;

    HashSet<GameObject> enemies = new HashSet<GameObject>();
    public GameObject[] spawns = new GameObject[3];
    private float randomSpawn;

    public PlayerDamage playerHealth;

    public TMP_Text enemyCounter;
    public TMP_Text waveReached;

    private bool running;
    private bool pauseGame;
    private bool pauseDebounce = true;

    public TimeManager timeManager;

    private bool perkDebounce;

    private float perkTimer;
    public float perkTimerLength;

    // Start is called before the first frame update
    void Start() {
        running = false;
    }

    // Update is called once per frame
    void FixedUpdate() {

        if (running) {
            Playing();

        }
    }

    public void resetGame() {
        wave = 0;

    }

    public void startGame() {
        pauseGame = false;
        running = true;

    }

    public void stopGame() {
        running = false;
    
    }

    public void enemyDeath(GameObject enemy) {
        enemies.Remove(enemy);
        enemiesKilled++;
    }

    public float EnemiesLeft() {
        return enemies.Count;

    }

    public bool WaveOver() {
        return pauseGame;

    }

    public bool isSpawningEnemies() {
        return spawningEnemies;

    }

    

    public void StartWave() {
        wave++;

        enemy.GetComponent<EnemyHealth>().addStatAmplifier(wave);

        running = true;
        pauseDebounce = true;

        waveCounter.text = "Wave " + wave;
        waveReached.text = "Reached Wave " + wave;

        intermission = false;
        waveCooldown = 0;

        enemiesSpawned = 0;
        enemiesPerWave = 2 * wave;

        spawningEnemies = true;

    }

    private void spawnEnemy() {
        GameObject newEnemy = Instantiate(enemy, spawns[((int)randomSpawn)].transform.position, Quaternion.identity);
        Enemy enemyProperties = newEnemy.GetComponent<Enemy>();
        enemyProperties.active = true;
        enemyAI enemyMovementProperties = newEnemy.GetComponent<enemyAI>();

        enemies.Add(newEnemy);

        if (UnityEngine.Random.Range(0, 5) == 4)
        {
            enemyMovementProperties.walkspeed /= 2;
            enemyMovementProperties.projectileFirerate += (UnityEngine.Random.Range(-100, 100) * 0.01f);
            enemyMovementProperties.canFireProjectiles = true;

        }

    }

    private void Playing() {
        if (enemies.Count > 0) {
            enemyCounter.text = "Enemies Left: " + enemies.Count;
        }
        if (timer >= spawnDelay && enemiesSpawned < enemiesPerWave && spawningEnemies) {
            timer = 0;
            randomSpawn = UnityEngine.Random.Range(0, 3);

            spawnEnemy();

            enemiesSpawned++;

            if (enemiesSpawned >= enemiesPerWave) {
                spawningEnemies = false;
            }
        }

        if (timer < spawnDelay)
        {
            timer += Time.fixedDeltaTime;

        }

    }
}
