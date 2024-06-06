using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Networking.PlayerConnection;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour {

    [SerializeField] Image title;
    [SerializeField] Button button;

    [SerializeField] GameObject player;
    private PlayerMovement movement;
    private PlayerDamage playerHealth;
    private PlayerCamera playerCamera;
    [SerializeField] UIManager UImanager;

    [SerializeField] PlayerMovement playerController;

    [SerializeField] Canvas playerHUD;
    [SerializeField] Canvas deathUI;
    [SerializeField] Canvas mainMenuUI;
    [SerializeField] MainMenuEnemyEffect enemySpawn;

    [SerializeField] Camera deathCam;
    [SerializeField] Camera playerCam;
    [SerializeField] Camera mainMenuCam;

    [SerializeField] GameObject leftArm;
    [SerializeField] GameObject rightArm;

    [SerializeField] GameObject VM;

    private int childCount;

    private bool viewingMenu;
    [SerializeField] WaveManager waves;

    [SerializeField] Image aboutMe;
    [SerializeField] Button aboutMeButton;
    [SerializeField] Button exitAboutMeButton;

    // Start is called before the first frame update
    void Start() {
        movement = player.GetComponent<PlayerMovement>();
        playerHealth = player.GetComponent<PlayerDamage>();
        playerCamera = player.GetComponent<PlayerCamera>();
        showMenu();
    }

    // Update is called once per frame
    void Update() {
        // debug
        if (Input.GetKey(KeyCode.P)) {
            showMenu();

        }

    }

    public bool viewingMainMenu() {
        return viewingMenu;

    }



    // start game
    public void help() {
        viewingMenu = false;

        deathCam.enabled = false;
        playerCam.enabled = true;
        mainMenuCam.enabled = false;

        movement.enabled = true;
        playerCamera.enabled = true;
        playerHUD.enabled = true;
        deathUI.enabled = false;

        playerController.ResetPlayerPosition();

        leftArm.transform.GetComponent<FireWeapon>().resetWeaponStats();
        rightArm.transform.GetComponent<FireWeapon>().resetWeaponStats();
        movement.setMovementStats(12, 5, "set");
        playerHealth.setMaxHealth(5, "set");
            
        waves.wave = 0;

        mainMenuUI.enabled = false;

        title.enabled = false;
        button.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        enemySpawn.turnOffEnemySpawning();

        UImanager.ShowPlayerHUD();

        VM.gameObject.SetActive(true);
    }

    public void showAboutMe() {
        aboutMe.gameObject.SetActive(true);
        aboutMeButton.interactable = false;
        exitAboutMeButton.gameObject.SetActive(true);
        exitAboutMeButton.GetComponent<Button>().interactable = true;

        aboutMe.enabled = true;

    }

    public void hideAboutMe() {
        aboutMe.gameObject.SetActive(false);
        exitAboutMeButton.gameObject.SetActive(false);
        aboutMeButton.interactable = true;

    }

    public void showMenu() {
        viewingMenu = true;

        deathCam.enabled = false;
        playerCam.enabled = false;
        mainMenuCam.enabled = true;

        player.transform.position = mainMenuCam.transform.position;

        title.enabled = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        button.gameObject.SetActive(true);

        mainMenuUI.enabled = true;

        movement.enabled = false;
        playerCamera.enabled = false;
        playerHUD.enabled = false;
        deathUI.enabled = false;

        enemySpawn.turnOnEnemySpawning();

        VM.gameObject.SetActive(false);

    }

    public void respawn() {
        help();
        

    }
}
