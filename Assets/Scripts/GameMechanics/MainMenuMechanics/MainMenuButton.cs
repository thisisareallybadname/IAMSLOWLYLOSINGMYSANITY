using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Networking.PlayerConnection;
using UnityEngine;
using UnityEngine.UI;

// manages main menu shenannigans (hides everything except menu stuff in specific circumstances)
public class MainMenuButton : MonoBehaviour {

    // you have no idea how awful it was to see all of these vars without comments
    // just [serializefield] spam because this script toggles so much stuff lmao
    // i use dark mode and i just see a wall of light mode here

    //-- main menu stuff
    [SerializeField] Image title;
    [SerializeField] Button button;

    //player fields
    [SerializeField] GameObject player;
    private PlayerMovement movement;
    private PlayerDamage playerHealth;
    private PlayerCamera playerCamera;
    [SerializeField] UIManager UImanager;

    // player behaviors
    [SerializeField] PlayerMovement playerController;
    [SerializeField] Canvas playerHUD;
    [SerializeField] Canvas deathUI;
    [SerializeField] Canvas mainMenuUI;
    [SerializeField] MainMenuEnemyEffect enemySpawn;
    [SerializeField] TimeManager timeManager;
    [SerializeField] LandmineSetter bombDropper;

    // all the cameras used in the game
    [SerializeField] Camera deathCam;
    [SerializeField] Camera playerCam;
    [SerializeField] Camera mainMenuCam;
    [SerializeField] Camera VMcamera;

    private bool startedTutorial;

    // arms 
    [SerializeField] GameObject leftArm;
    [SerializeField] GameObject rightArm;

    [SerializeField] GameObject VM;

    [SerializeField] GameObject originalEnemy;

    // enemy properties
    private EnemyMovement enemyMovement;
    private EnemyHealth enemyHealth;


    private bool viewingMenu;

    // game mechanics
    [SerializeField] WaveManager waves;
    [SerializeField] PerkManager perkManager;

    [SerializeField] Image aboutMe;
    [SerializeField] Button aboutMeButton;
    [SerializeField] Button exitAboutMeButton;

    // Start is called before the first frame update
    void Start() {
        movement = player.GetComponent<PlayerMovement>();
        playerHealth = player.GetComponent<PlayerDamage>();
        playerCamera = player.GetComponent<PlayerCamera>();
        showMenu();

        enemyMovement = originalEnemy.GetComponent<EnemyMovement>();
        enemyHealth = originalEnemy.GetComponent<EnemyHealth>();
    }

    // Update is called once per frame
    void Update() {
        if (viewingMenu) {
            deathCam.enabled = false;
            deathUI.enabled = false;
        }

    }

    // basic getters
    public bool viewingMainMenu() {
        return viewingMenu;

    }

    // start game
    public void startGame() {
        viewingMenu = false;

        // enable player cams, disable everything else
        deathCam.enabled = false;
        playerCam.enabled = true;
        mainMenuCam.enabled = false;
        VMcamera.enabled = true;
           
        // enable important player scripts/behaviors
        movement.enabled = true;
        playerCamera.enabled = true;
        playerHUD.enabled = true;
        deathUI.enabled = false;
        UImanager.enabled = true;

        bombDropper.resetLandmineCount();

        // make player go back to spawn point
        playerController.SetPosition(new Vector3(0, 10, 0));

        // reset stats of all stuff
        leftArm.GetComponent<FireWeapon>().SetWeaponStats(1, 1/3f, "set");
        rightArm.GetComponent<FireWeapon>().SetWeaponStats(3, 1, "set");
        movement.setMovementStats(12, 5, "set");
        playerHealth.setMaxHealth(5, "set");

        perkManager.setSelectedPerkStatus(false);
        

        // reset wave and enemy stuff
        waves.setWave(0);
        enemyHealth.setHealth(5, "set");
        enemyMovement.setWalkspeed(6, "set");

        // disable main menu ui
        mainMenuUI.enabled = false;
        title.enabled = false;
        button.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        enemySpawn.turnOffEnemySpawning();
        UImanager.ShowPlayerHUD();

        VM.SetActive(true);
        if (startedTutorial){
            timeManager.StartGame();

        }

        startedTutorial = true;
    }

    // makes about me and close about me button visible
    // i dont have to worry about disabling the play button and stuff because they'll get covered
    // and won't register getting pressed if they have ui element over them
    public void showAboutMe() {
        aboutMe.gameObject.SetActive(true);
        aboutMeButton.interactable = false;
        exitAboutMeButton.gameObject.SetActive(true);
        exitAboutMeButton.GetComponent<Button>().interactable = true;

        aboutMe.enabled = true;

    }

    // hide about me UI
    // activated by x button in about me 
    public void hideAboutMe() {
        aboutMe.gameObject.SetActive(false);
        exitAboutMeButton.gameObject.SetActive(false);
        aboutMeButton.interactable = true;

    }

    // show all menu UI, hide everything else
    public void showMenu() {
        viewingMenu = true;

        UImanager.enabled = false;

        // enable cameras
        deathCam.enabled = false;
        playerCam.enabled = false;
        mainMenuCam.enabled = true;
        VMcamera.enabled = true;

        title.enabled = true;

        // make cursor lock at screen
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        button.gameObject.SetActive(true);

        mainMenuUI.enabled = true;

        playerController.SetPosition(new Vector3(-13.5f, 79, -68.3f));

        // disable all player stuff
        movement.enabled = false;
        playerCamera.enabled = false;
        playerHUD.enabled = false;
        deathUI.enabled = false;

        enemySpawn.turnOnEnemySpawning();

        VM.SetActive(false);

        deathCam.enabled = false;


    

    }

    public void respawn() {
        startedTutorial = true;
        startGame();

    }
}
