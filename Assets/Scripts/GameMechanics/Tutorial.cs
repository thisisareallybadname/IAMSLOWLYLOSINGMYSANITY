using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

// teaches player how to play game
public class Tutorial : MonoBehaviour {

    // important fields for setting up tutorial
    [SerializeField] GameObject enemy;
    [SerializeField] WaveManager waveManager;
    [SerializeField] TimeManager timeManager;
    [SerializeField] MainMenuButton mainMenu;
    [SerializeField] PerkManager perkManager;

    // detects which parts player finished
    [SerializeField] bool finishedPart1;
    [SerializeField] bool finishedPart2;
    [SerializeField] bool finishedPart3;
    [SerializeField] bool finishedPart4;

    [SerializeField] TMP_Text enemiesLeft; // thingy in player hud that shows how much enemies are left
    [SerializeField] TMP_Text waveCounter;
    [SerializeField] Image enemiesLeftBG;
    [SerializeField] Image waveCounterBG;

    private bool spawnedLandmine;

    private bool spawnedTargetEnemy = false;
    private bool spawnedPerks = false;

    private float movementDeltaCounter;
    private float mouseDeltaCounter;

    [SerializeField] GameObject targetEnemy;
    [SerializeField] GameObject enemySpawn;
    [SerializeField] Material targetMaterial;

    private GameObject spawnedEnemy;
    private bool dashed;

    // tutorial box
    [SerializeField] TMP_Text instructions;
    [SerializeField] Image instructionsBox;

    // landmine stuff
    [SerializeField] GameObject landmine;
    private GameObject tutorialLandmine;

    // Start is called before the first frame update
    void Start() {
        instructionsBox.enabled = false;
        instructions.enabled = false;

    }

    // shows how to move
    private void tutorialPart1() {
        if (!finishedPart1) {
            instructions.text = "use wasd to move around, space to jump, and left shift to dash!! (try it out)";

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

    // teaches how to use weapon
    private void tutorialPart2() {
        instructions.text = "oh look a target I spent two minutes on magaically " +
            "materialized out of thin air! destroy it with your " +
            "awfully-modeled weapons! (LMB for crossbow, RMB for cannon)";

        if (!spawnedTargetEnemy) {

            // set enemy stats
            spawnedEnemy = Instantiate(enemy, enemySpawn.transform.position, Quaternion.identity);
            spawnedEnemy.GetComponent<EnemyHealth>().enabled = true;
            spawnedEnemy.GetComponent<EnemyHealth>().setHealth(5, "set");
            spawnedEnemy.GetComponent<EnemyMovement>().enabled = true;
            spawnedEnemy.GetComponent<EnemyMovement>().setWalkspeed(0, "set");
            spawnedEnemy.gameObject.SendMessage("changeSprite", targetMaterial);
            spawnedTargetEnemy = true;
        }


        if (spawnedEnemy == null && spawnedTargetEnemy) {
            finishedPart2 = true;

        }

    }

    // shows landmines
    private void tutorialPart3() {

        // make dummy landmine
        if (!spawnedLandmine) {
            spawnedLandmine = true;
            instructions.text = "a wild landmine appeared! these landmines spawn in every wave," +
                " and at the wooden signs!!!!! step on it/shoot it to see what happens!!!";
            tutorialLandmine = 
                Instantiate(landmine, enemySpawn.transform.position, Quaternion.identity);

            // activate landmine
            ProjectileBehavior landmineBehavior = 
                tutorialLandmine.GetComponent<ProjectileBehavior>();

            landmineBehavior.enabled = true;

        }


        if (tutorialLandmine == null && spawnedLandmine) {
            finishedPart3 = true;

        }

    }

    // shows perk system
    private void tutorialPart4() {

        // spawn perks
        instructions.text = 
        "shoot one of the perk enemies to get a new perk and finish the tutorial!";
        if (!spawnedPerks)
        {
            timeManager.StartGame();
            spawnedPerks = true;
        }


        if (perkManager.PlayerSelectedPerk()) {
           finishedPart4 = true;


        }

    }

    // Update is called once per frame
    void Update() {
        if (!mainMenu.viewingMainMenu()) {

            // hide tutorial ui once tutorial is finished
            if (!finishedPart4) {
                instructionsBox.enabled = true;
                instructions.enabled = true;


            } else {
                instructionsBox.enabled = false;
                instructions.enabled = false;

            }
            
            // runs parts in chronological order
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
