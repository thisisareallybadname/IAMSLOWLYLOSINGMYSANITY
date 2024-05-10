using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {

    public GameObject enemy;
    public Transform[] spawnpoints;
    public WaveManager waveManager;
    public Material[] enemySprites; 

    private GameObject tutorialEnemy;
    private GameObject MovementTutorial;
    private GameObject waveTutorial;

    public bool skipTutorial;

    // Start is called before the first frame update
    void Start() {
        tutorialEnemy = Instantiate(enemy, spawnpoints[1].position, Quaternion.identity);
        tutorialEnemy.GetComponent<Enemy>().active = false;
        tutorialEnemy.GetComponent<Enemy>().speed = 0;


        MovementTutorial = Instantiate(enemy, spawnpoints[0].position, Quaternion.identity);
        MovementTutorial.GetComponent<Enemy>().active = false;
        MovementTutorial.GetComponent<Enemy>().speed = 0;

        MovementTutorial.GetComponent<EnemyHealth>().health = 1000000;
        MovementTutorial.GetComponent<EnemyHealth>().kbResistance = 1000000;
        MovementTutorial.GetComponent<EnemyHealth>().FullHealthSprite = enemySprites[1];

        waveTutorial = Instantiate(enemy, spawnpoints[2].position, Quaternion.identity);
        waveTutorial.GetComponent<Enemy>().active = false;
        waveTutorial.GetComponent<Enemy>().speed = 0;

        waveTutorial.GetComponent<EnemyHealth>().health = 1000000;
        waveTutorial.GetComponent<EnemyHealth>().kbResistance = 1000000;
        waveTutorial.GetComponent<EnemyHealth>().FullHealthSprite = enemySprites[2];

        if (skipTutorial) {
            Destroy(tutorialEnemy);

        }
    }

    // Update is called once per frame
    void Update() {
        if (tutorialEnemy == null) {
            waveManager.startGame = true;
            if (MovementTutorial != null && waveTutorial != null) {
                MovementTutorial.GetComponent<EnemyHealth>().kbResistance = 1;
                waveTutorial.GetComponent<EnemyHealth>().kbResistance = 1;

                MovementTutorial.GetComponent<EnemyHealth>().takeDamage(1000000, 0);
                waveTutorial.GetComponent<EnemyHealth>().takeDamage(1000000, 0);
            }
        }

    }
}
