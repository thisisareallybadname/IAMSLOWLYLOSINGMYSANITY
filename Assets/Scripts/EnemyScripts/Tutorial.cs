using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {

    public GameObject enemy;
    public Transform spawnpoint;
    public WaveManager waveManager;

    private GameObject tutorialEnemy;

    // Start is called before the first frame update
    void Start() {
        tutorialEnemy = Instantiate(enemy, spawnpoint.position, Quaternion.identity);
        tutorialEnemy.GetComponent<enemyAI>().chasePlayer = true;
        tutorialEnemy.GetComponent<enemyAI>().walkspeed = 0;
    }

    // Update is called once per frame
    void Update() {
        if (tutorialEnemy == null) {
            waveManager.startGame = true;

        }

    }
}
