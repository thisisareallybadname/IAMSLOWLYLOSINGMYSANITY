using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// manages button shenannigans (hides everything except menu stuff in specific circumstances)
public class ButtonManager : MonoBehaviour {

    // you have no idea how awful it was to see all of these vars without comments
    // just [serializefield] spam because this script toggles so much stuff lmao
    // i use dark mode and i just see a wall of light mode here

    //-- main menu stuff
    [SerializeField] Image title;

    // buttons
    [SerializeField] Button button;

    //player fields
    [SerializeField] GameObject player;
    private PlayerMovement movement;
    private PlayerDamage playerHealth;
    private PlayerCamera playerCamera;
    [SerializeField] UIManager UImanager;

    // important behaviors
    [SerializeField] PlayerMovement playerController;
    [SerializeField] MainMenuEnemyEffect enemySpawn;
    [SerializeField] TimeManager timeManager;
    [SerializeField] LandmineSetter bombDropper;
    [SerializeField] Tutorial tutorial;
       
    // canvases
    [SerializeField] Canvas playerHUD;
    [SerializeField] Canvas deathUI;
    [SerializeField] Canvas mainMenuUI;
    [SerializeField] Canvas tutorialUI;
    
    // all the cameras used in the game
    [SerializeField] Camera deathCam;
    [SerializeField] Camera playerCam;
    [SerializeField] Camera mainMenuCam;
    [SerializeField] Camera VMcamera;

    private bool finishedTutorial;

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
    [SerializeField] PauseGame pauseMenu;

    // stuff that pops up from button
    
    // about me
    [SerializeField] Image aboutMe;
    [SerializeField] Button aboutMeButton;
    [SerializeField] Button exitAboutMeButton;

    // controls
    [SerializeField] Image controls;
    [SerializeField] Button controlsButton;
    [SerializeField] Button exitControlsButton;

    // [REDACTED
    [SerializeField] Image REDACTED;

    // Start is called before the first frame update
    void Start() {
        movement = player.GetComponent<PlayerMovement>();
        playerHealth = player.GetComponent<PlayerDamage>();
        playerCamera = player.GetComponent<PlayerCamera>();
        showMenu();

        enemyMovement = originalEnemy.GetComponent<EnemyMovement>();
        enemyHealth = originalEnemy.GetComponent<EnemyHealth>();

        Application.targetFrameRate = 145;
    }

    // Update is called once per frame
    void Update() {
        if (viewingMenu) {
            deathCam.enabled = false;
            deathUI.enabled = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

    }

    // basic getters
    public bool viewingMainMenu() {
        return viewingMenu;

    }

    public void PauseGame() {
        Time.timeScale = 0;

    }

    public void UnpauseGame() {
        Time.timeScale = 1;

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
        pauseMenu.enabled = true;
        tutorialUI.enabled = true;

        bombDropper.resetMinefield();

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
        if (finishedTutorial){
            timeManager.StartGame();

        }

        finishedTutorial = true;
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

    // same thing as about me stuff but for controls
    public void showControls() {
        controls.gameObject.SetActive(true);
        controlsButton.interactable = false;
        exitControlsButton.gameObject.SetActive(true);
        exitControlsButton.GetComponent<Button>().interactable = true;

        controls.enabled = true;

    }

    public void hideControls() {
        controls.gameObject.SetActive(false);
        exitControlsButton.gameObject.SetActive(false);
        controlsButton.interactable = true;

    }

    public void QuitGame() {
        REDACTED.enabled = true;
        Application.Quit();
    }

    // show all menu UI, hide everything else
    public void showMenu() {

        waves.clearWave();
        perkManager.forceQuitPerkSelection();

        /*
        if (!tutorial.finishedTutorial(3) && !finishedTutorial) {
            finishedTutorial = false;

        }
        */

        viewingMenu = true;

        UImanager.enabled = false;

        // enable cameras
        deathCam.enabled = false;
        playerCam.enabled = false;
        mainMenuCam.enabled = true;
        VMcamera.enabled = true;

        title.enabled = true;

        button.gameObject.SetActive(true);

        mainMenuUI.enabled = true;

        playerController.SetPosition(new Vector3(-13.5f, 79, -68.3f));

        // disable all player stuff
        movement.enabled = false;
        playerCamera.enabled = false;
        playerHUD.enabled = false;
        deathUI.enabled = false;
        tutorialUI.enabled = false;

        enemySpawn.turnOnEnemySpawning();

        VM.SetActive(false);

        deathCam.enabled = false;



        // make cursor lock at screen
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void respawn() {
        finishedTutorial = true;
        startGame();

    }
}
