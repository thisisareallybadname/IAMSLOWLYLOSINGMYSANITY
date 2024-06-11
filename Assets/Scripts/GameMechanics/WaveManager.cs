using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

// summons waves of enemies for player to fight
public class WaveManager : MonoBehaviour {

    // used to keep track of spawn delay
    private float timer;
    [SerializeField] float spawnDelay;
    
    [SerializeField] float enemiesPerWave;
    private float enemiesSpawned;

    [SerializeField] bool spawningEnemies = false;

    [SerializeField] GameObject enemy;

    [SerializeField] float wave = 0;
    [SerializeField] TMP_Text waveCounter; // thingy in "Wave : [wave]" ui

    HashSet<GameObject> enemies = new HashSet<GameObject>(); // keeps track of enemy count
    [SerializeField] GameObject[] spawns = new GameObject[3]; // holds spawn positions
    private float randomSpawn;

    [SerializeField] Image waveCompletedImage;
    [SerializeField] Image enemiesLeft;

    // ui elements 
    [SerializeField] TMP_Text enemyCounter;
    [SerializeField] TMP_Text waveReached;

    private bool running;

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
    

    // bunch of setters and getters
    public void startGame() {
        running = true;

    }

    public void stopGame() {
        running = false;
    
    }

    // used to update enemies list
    public void enemyDeath(GameObject enemy) {
        enemies.Remove(enemy);
    }

    public float EnemiesLeft() {
        return enemies.Count;

    }

    public bool isSpawningEnemies() {
        return spawningEnemies;

    }

    public void clearWave(){
        wave = 0;
        waveCounter.text = "Wave 1";
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<EnemyHealth>().takeDamage(10000, 0);

        }

    }
    
    public void StartWave() {
        wave++;

        running = true;

        waveCounter.text = "Wave " + wave;
        waveReached.text = "Reached Wave " + wave;

        enemiesSpawned = 0;
        enemiesPerWave = 2 * wave;

        spawningEnemies = true;

    }

    // spawns enemy (pretty self explanatory ngl)
    private void spawnEnemy() {
        GameObject newEnemy = Instantiate(enemy, spawns[((int)randomSpawn)].transform.position, Quaternion.identity);
        newEnemy.GetComponent<EnemyHealth>().enabled = true;
        newEnemy.GetComponent<EnemyMovement>().enabled = true;

        //newEnemy.GetComponent<EnemyHealth>().setHealth(5, "set");

        enemies.Add(newEnemy);

        // 1/4 chance for enemy to be ranged enemy
        if (UnityEngine.Random.Range(0, 4) == 0) {
            newEnemy.SendMessage("EnableRangedAttack");
            newEnemy.GetComponent<EnemyMovement>().setWalkspeed(0.5f, "multi");

        }

    }
    public bool isRunning(){
        return running;

    }

    private void Playing() {

        // update ui
        if (enemies.Count > 0) {
            enemyCounter.text = "Enemies Left: " + enemies.Count;
        }

        // spawn enemies every [spawnDelay] seconds
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

    public float getWave() { 
        return wave; 
    
    }

    public void setWave(float newWave) {
        wave = newWave;
        print(wave);
    }
}
