using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{

    public Transform[] tutorialSpawnPoints;
    public Texture[] textures;

    public GameObject enemy;

    private GameObject WASDenemy;
    private GameObject JUMPenemy;
    private GameObject proceedEnemy;

    private GameObject exampleEnemy;
    private GameObject exampleEnemyText;
    private GameObject proceedEnemy2;

    private bool skipTutorial;

    public WaveManager waveManager;

    // Start is called before the first frame update
    void Start() {
        

    }

    private GameObject spawnTutorialEnemy(GameObject clone, Texture instructions, Transform spawnPosition) {
        GameObject enemy = Instantiate(clone, spawnPosition.position, Quaternion.identity);
        enemy.GetComponent<enemyAI>().chasePlayer = false;
        return enemy;
    }

    private void TutorialSection1() {
        WASDenemy = spawnTutorialEnemy(enemy, textures[0], tutorialSpawnPoints[0]);
        JUMPenemy = spawnTutorialEnemy(enemy, textures[1], tutorialSpawnPoints[1]);
        proceedEnemy = spawnTutorialEnemy(enemy, textures[2], tutorialSpawnPoints[2]);

        if (proceedEnemy.GetComponent<EnemyHealth>().health <= 0) {
            Destroy(WASDenemy);
            Destroy(JUMPenemy);

            TutorialSection2();

        }

    }

    private void TutorialSection2() {
        exampleEnemy = spawnTutorialEnemy(enemy, textures[3], tutorialSpawnPoints[1]);
        exampleEnemyText = spawnTutorialEnemy(enemy, textures[4], tutorialSpawnPoints[2]);
        proceedEnemy2 = spawnTutorialEnemy(enemy, textures[5], tutorialSpawnPoints[3]);

        if (proceedEnemy2.GetComponent<EnemyHealth>().health <= 0) {
            skipTutorial = true;

        }

    }

    // Update is called once per frame
    void Update() {
        if (skipTutorial) {
            waveManager.startGame = true;

        }

    }
}