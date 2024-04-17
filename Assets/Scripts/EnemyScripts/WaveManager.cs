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

    HashSet<Enemy> enemies = new HashSet<Enemy>();

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
        for (int i = 0; i < enemiesPerWave; i++) {
            spawner.spawnEnemy("a", 1, 1, 1);
            yield return new WaitForSeconds(spawnDelay);

        }

        yield return null;
    }
}
