using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// spawns falling enemy effect in main menu
public class MainMenuEnemyEffect : MonoBehaviour {

    [SerializeField] GameObject enemy;
    [SerializeField] GameObject target;
    private float timer;
    private bool spawnEnemies = true;

    // Start is called before the first frame update
    void Start() {
        spawnEnemies = true;

    }

    // Update is called once per frame
    void Update() {
        // summon enemy every 1/4th of a second
        if (spawnEnemies) {
            if (timer < 0.25f) {
                timer += Time.deltaTime;

            }  else {
                timer = 0;
                StartCoroutine(spawnEnemy());

            }
        }
    }

    // spawn enemy every 1/4 of a second
    public IEnumerator spawnEnemy() {
        GameObject newEnemy = 
        Instantiate(enemy, transform.position + transform.right * Random.Range(-15, 15), Quaternion.identity);
        newEnemy.transform.LookAt(target.transform);

        // instantly kill enemy 
        newEnemy.GetComponent<EnemyHealth>().enabled = true;
        newEnemy.GetComponent<EnemyHealth>().setLieOnFloorTime(3);
        newEnemy.GetComponent<EnemyMovement>().enabled = false;

        // chance for enemy to be a ranged one
        if (Random.Range(0, 4) == 1) {
            newEnemy.SendMessage("EnableRangedAttack");

        }

        // kill the enemy
        newEnemy.GetComponent<EnemyHealth>().takeDamage(10000000, 5);
        yield return new WaitForSeconds(1.5f);
    }

    // enable/disable effect
    public void turnOffEnemySpawning() {
        spawnEnemies = false;
        
    }

    public void turnOnEnemySpawning() {
        spawnEnemies = true;

    }
        
}
