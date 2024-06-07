using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

    public GameObject enemy;
    public WaveManager waveManager;
    public TimeManager timeManager;
    public MainMenuButton mainMenu;
    public PerkManager perkManager;

    [SerializeField] bool finishedPart1;
    [SerializeField] bool finishedPart2;
    [SerializeField] bool finishedPart3;
    [SerializeField] bool finishedPart4;

    private bool spawnedLandmine;

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

    public GameObject landmine;
    private GameObject tutorialLandmine;

    // Start is called before the first frame update
    void Start() {
        instructionsBox.enabled = false;
        instructions.enabled = false;

    }

    private void tutorialPart1() {
        if (!finishedPart1) {
            instructions.text = "uh wasd to move, move mouse to move camera, space to jump, lshift to dash (try it out!!!!!)";

            movementDeltaCounter += (Mathf.Abs(Input.GetAxisRaw("Horizontal")) + Mathf.Abs(Input.GetAxisRaw("Vertical")) + Input.GetAxisRaw("Jump")) / 2f;
            mouseDeltaCounter += Input.mousePositionDelta.magnitude / 5f;

            if (Input.GetButton("Fire3") && !dashed)
            {
                dashed = true;

            }
        } 
        if (movementDeltaCounter > 100 && mouseDeltaCounter > 100 && dashed)
        {
            finishedPart1 = true;

        }
    }

    private void tutorialPart2() {
        instructions.text = "oh look a target I spent two minutes on magaically materialized out of nowhere! shoot it with your weapons!!11! (LMB for crossbow, RMB for cannon)";

        if (!spawnedTargetEnemy) {
            spawnedEnemy = Instantiate(enemy, enemySpawn.transform.position, Quaternion.identity);
            spawnedEnemy.GetComponent<EnemyHealth>().enabled = true;
            spawnedEnemy.GetComponent<EnemyHealth>().setHealth(5);
            spawnedEnemy.GetComponent<EnemyMovement>().enabled = true;
            spawnedEnemy.GetComponent<EnemyMovement>().setWalkspeed(0, "set");
            spawnedEnemy.gameObject.SendMessage("changeSprite", targetMaterial);
            spawnedTargetEnemy = true;
        }


        if (spawnedEnemy == null && spawnedTargetEnemy) {
            finishedPart2 = true;

        }

    }

    private void tutorialPart3() {

        if (!spawnedLandmine) {
            spawnedLandmine = true;
            instructions.text = "a wild landmine appeared! these landmines spawn in every wave, and at the wooden signs!!!!! step on it/shoot it to see what happens!!!";
            tutorialLandmine = Instantiate(landmine, enemySpawn.transform.position, Quaternion.identity);
            ProjectileBehavior landmineBehavior = tutorialLandmine.GetComponent<ProjectileBehavior>();

            landmineBehavior.enabled = true;

        }


        if (tutorialLandmine == null && spawnedLandmine) {
            finishedPart3 = true;

        }

    }

    private void tutorialPart4() {
        instructions.text = "Pick a perk at the pick-a-perk station! however, pick wisely as the perks aren't very balanced!";
        if (!spawnedPerks)
        {
            timeManager.StartGame();
            spawnedPerks = true;
        }


        if (perkManager.PlayerSelectedPerk())
        {
            finishedPart4 = true;

        }

    }

    // Update is called once per frame
    void Update() {
        if (!mainMenu.viewingMainMenu()) {
            if (!finishedPart4) {
                instructionsBox.enabled = true;
                instructions.enabled = true;


            } else {
                instructionsBox.enabled = false;
                instructions.enabled = false;

            }
            
            if (!finishedPart1) {
                tutorialPart1();

            } else if (!finishedPart2) {
                tutorialPart2();

            } else if (!finishedPart3) {
                tutorialPart3();

            } else if (!finishedPart4) {
                tutorialPart4();

            }

        }
    }
}
