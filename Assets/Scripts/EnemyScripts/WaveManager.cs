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

    public float enemiesPerWave;

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
        if (timer > waveDelay) {
            StartCoroutine(spawnWave());
            timer = -123123;
        }

        timer += Time.fixedDeltaTime;
    }

    private IEnumerator spawnWave() {

        randomSpawn = UnityEngine.Random.Range(0, 3);
        for (int i = 0; i < enemiesPerWave; i++) {
            GameObject newEnemy = Instantiate(enemy, spawns[((int)randomSpawn)].transform.position, Quaternion.identity);
            enemies.Add(newEnemy);
            yield return new WaitForSeconds(spawnDelay);

        }

        yield return null;
    }

    public void enemyDeath(GameObject enemy) {
        enemies.Remove(enemy);

    }
}
