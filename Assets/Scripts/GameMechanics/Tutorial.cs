using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

    public GameObject enemy;
    public WaveManager waveManager;
    public TimeManager timeManager;
    public MainMenuButton mainMenu;
    public PerkManager perkManager;

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
    private bool dashed;

    public bool skipTutorial;
    public bool tutorialFinished;

    public TMP_Text instructions;
    public Image instructionsBox;
    private bool startedGame = false;

    // Start is called before the first frame update
    void Start() {
        instructionsBox.enabled = false;

    }

    // Update is called once per frame
    void Update() {
        if (!mainMenu.viewingMainMenu()) {
            if (!finishedPart3) {
                instructionsBox.enabled = true;
                instructions.enabled = true;


            } else {
                instructionsBox.enabled = false;
                instructionsBox.enabled = false;

            }
            
            if (!finishedPart1) {

                instructions.text = "Use WASD to walk around, SPACE to jump, and move your mouse to move the camera. Press LSHIFT to use some stamina to dash.";

                movementDeltaCounter += (Mathf.Abs(Input.GetAxisRaw("Horizontal")) + Mathf.Abs(Input.GetAxisRaw("Vertical")) + Input.GetAxisRaw("Jump")) / 2f;
                mouseDeltaCounter += Input.mousePositionDelta.magnitude / 25f;

                if (Input.GetButton("Fire3") && !dashed) {
                    dashed = true;

                }

                if (movementDeltaCounter > 100 && mouseDeltaCounter > 100 && dashed)
                {
                    finishedPart1 = true;

                }
            }
            else if (!finishedPart2) {
                instructions.text = "Shoot left weapon with LMB, shoot right one with RMB. shoot down that target over there";

                if (!spawnedTargetEnemy) {
                    spawnedEnemy = Instantiate(enemy, enemySpawn.transform.position, Quaternion.identity);
                    spawnedEnemy.GetComponent<EnemyHealth>().changeSprite(targetMaterial);
                    spawnedTargetEnemy = true;
                }


                if (spawnedEnemy == null) {
                    finishedPart2 = true;
                    

                }

            } else if (!finishedPart3) {
                instructions.text = "Pick a perk at the pick-a-perk station! however, pick wisely as the perks aren't very balanced!";
                if (!spawnedPerks) {
                    timeManager.StartGame();
                    spawnedPerks = true;

                    Debug.Log("test");
                }
                

                if (perkManager.PlayerSelectedPerk()) {
                    finishedPart3 = true;

                }
            }

        } else {
            instructions.enabled = false;

        }
    }
}
