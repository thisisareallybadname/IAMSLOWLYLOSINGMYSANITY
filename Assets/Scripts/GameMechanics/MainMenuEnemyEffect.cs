using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuEnemyEffect : MonoBehaviour {

    public GameObject enemy;
    private float timer;
    private bool spawnEnemies = true;

    // Start is called before the first frame update
    void Start() {
        spawnEnemies = true;

    }

    // Update is called once per frame
    void Update() {
        if (spawnEnemies) {
            if (timer < 0.05f) {
                timer += Time.deltaTime;

            }  else {
                timer = 0;
                StartCoroutine(spawnEnemy());

            }
        }
    }

    public IEnumerator spawnEnemy() {
        GameObject newEnemy = Instantiate(enemy, transform.position + transform.right * Random.Range(-15, 15), Quaternion.identity) as GameObject;
        newEnemy.GetComponent<EnemyHealth>().lieOnFloorMaxTime = 3;
        newEnemy.GetComponent<EnemyHealth>().takeDamage(10000000, 15);
        yield return null;
    }

    public void turnOffEnemySpawning() {
        spawnEnemies = false;
        
    }
        
}
