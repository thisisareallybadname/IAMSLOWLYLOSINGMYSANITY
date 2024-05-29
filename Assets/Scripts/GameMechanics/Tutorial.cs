using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Tutorial : MonoBehaviour {

    public GameObject enemy;
    public Transform[] spawnpoints;
    public WaveManager waveManager;
    public TimeManager timeManager;
    public Material[] enemySprites;
    public TimeManager time;
    public MainMenuButton mainMenu;

    private bool finishedPart1;
    private bool finishedPart2;
    private bool finishedPart3;

    private bool spawnedTargetEnemy = false;
    private bool spawnedPerks = false;

    private float movementDeltaCounter;
    private float mouseDeltaCounter;

    public GameObject targetEnemy;
    public GameObject enemySpawn;
    public Material targetMaterial;

    private GameObject spawnedEnemy;

    public bool skipTutorial;
    public bool tutorialFinished;

    public TMP_Text instructions;
    private bool startedGame = false;

    // Start is called before the first frame update
    void Start() {
        

    }

    // Update is called once per frame
    void Update() {
        if (!mainMenu.viewingMainMenu()) {
            if (!finishedPart2) {
                instructions.enabled = true;
            
            } else {
                instructions.enabled = false;

            }
            
            if (!finishedPart1) {

                instructions.text = "Move around with WASD, press SPACE to jump, look around by moving mouse";

                movementDeltaCounter += (Mathf.Abs(Input.GetAxisRaw("Horizontal")) + Mathf.Abs(Input.GetAxisRaw("Vertical")) + Input.GetAxisRaw("Jump")) / 5f;
                mouseDeltaCounter += Input.mousePositionDelta.magnitude / 25f;

                Debug.Log("part 1");
                Debug.Log(movementDeltaCounter);
                Debug.Log(mouseDeltaCounter);

                if (movementDeltaCounter > 100 && mouseDeltaCounter > 100)
                {
                    finishedPart1 = true;

                }
            }
            else if (!finishedPart2)
            {
                instructions.text = "Shoot left weapon with LMB, shoot right one with RMB. shoot down that target over there";

                if (!spawnedTargetEnemy) {
                    spawnedEnemy = Instantiate(enemy, enemySpawn.transform.position, Quaternion.identity);
                    spawnedEnemy.GetComponent<EnemyHealth>().changeSprite(targetMaterial);
                    spawnedTargetEnemy = true;
                }


                if (spawnedEnemy == null) {
                    finishedPart2 = true;
                    timeManager.StartGame();

                }

            }
        } else {
            instructions.enabled = false;

        }
    }
}
