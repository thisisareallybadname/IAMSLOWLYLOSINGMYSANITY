using System.Collections;
using System.Collections.Generic;
using UnityEditor.Networking.PlayerConnection;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour {

    public Image title;
    public Button button;

    public GameObject player;
    private PlayerMovement movement;
    private PlayerCamera playerCamera;
    public UIManager UImanager;
    
    public Canvas playerHUD;
    public Canvas deathUI;
    public Canvas mainMenuUI;
    public MainMenuEnemyEffect enemySpawn;

    public Camera deathCam;
    public Camera playerCam;
    public Camera mainMenuCam;

    public GameObject leftArm;
    public GameObject rightArm;

    public GameObject VM;

    private int childCount;

    private bool viewingMenu;
    public WaveManager waves;

    public GameObject aboutMe;
    public GameObject everythingElse;

    // Start is called before the first frame update
    void Start() {
        movement = player.GetComponent<PlayerMovement>();
        playerCamera = player.GetComponent<PlayerCamera>();
        showMenu();
    }

    // Update is called once per frame
    void Update() {
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

        player.GetComponent<PlayerDamage>().setHealth(5);
        player.GetComponent<PlayerDamage>().maxHealth = 5;
        leftArm.transform.GetChild(0).GetComponent<FireWeapon>().damage = 1;
        rightArm.transform.GetChild(0).GetComponent<FireWeapon>().damage = 1;
        movement.walkspeed = 12;
        waves.wave = 0;
        

        title.enabled = false;
        button.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        movement.enabled = true;
        playerCamera.enabled = true;
        playerHUD.enabled = true;
        deathUI.enabled = false;

        enemySpawn.turnOffEnemySpawning();

        UImanager.ShowPlayerHUD();

        VM.gameObject.SetActive(true);
    }

    public void showAboutMe() {

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

        movement.enabled = false;
        playerCamera.enabled = false;
        playerHUD.enabled = false;
        deathUI.enabled = false;

        enemySpawn.turnOnEnemySpawning();

        VM.gameObject.SetActive(false);

    }

    public void respawn() {
        Debug.Log("help");
        showMenu();

    }
}
