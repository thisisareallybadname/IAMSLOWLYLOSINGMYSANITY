using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuEnemyEffect : MonoBehaviour {

    [SerializeField] GameObject enemy;
    private float timer;
    private bool spawnEnemies = true;

    // Start is called before the first frame update
    void Start() {
        spawnEnemies = true;

    }

    // Update is called once per frame
    void Update() {

        Application.targetFrameRate = 60;

        if (spawnEnemies) {
            if (timer < 0.25f) {
                timer += Time.deltaTime;

            }  else {
                timer = 0;
                StartCoroutine(spawnEnemy());

            }
        }
    }
    public IEnumerator spawnEnemy() {
        GameObject newEnemy = Instantiate(enemy, transform.position + transform.right * Random.Range(-15, 15), Quaternion.identity);
        newEnemy.GetComponent<EnemyHealth>().enabled = true;
        newEnemy.GetComponent<EnemyHealth>().lieOnFloorMaxTime = 3;
        if (Random.Range(0, 4) == 1) {
            newEnemy.SendMessage("EnableRangedAttack");

        }

        newEnemy.GetComponent<EnemyHealth>().takeDamage(10000000, Random.Range(-15, 15));
        yield return null;
    }

    public void turnOffEnemySpawning() {
        spawnEnemies = false;
        
    }

    public void turnOnEnemySpawning() {
        spawnEnemies = true;

    }
        
}
