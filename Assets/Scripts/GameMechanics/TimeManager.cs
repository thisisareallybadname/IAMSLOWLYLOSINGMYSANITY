using System.Collections;
using System.Collections.Generic;
using System.Security;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TimeManager : MonoBehaviour {

    public WaveManager waves;
    public Tutorial tutorial;
    public PerkManager perkManager;
    public LandmineSetter landmines;
    public PlayerDamage playerHealth;
    public MainMenuButton mainMenu;
    public UIManager UIManager;

    private bool startGame;
    private bool gameOver;
    public bool spawnBombs;
    private bool pauseDebounce;
    public bool pauseGame;
    private bool playerInMenu;

    public float timer;
    private float waveCooldown;
    private float waveDelay = 7;
    public bool intermission;

    private float waveCompleteTimer;
    private bool showPerks = false;

    public TMP_Text enemiesLeft;

    // Start is called before the first frame update
    public void PauseGame() {
        waves.stopGame();

    }

    public void StartGame() {
        Debug.Log("tutorial finished");
        pauseDebounce = true;
        startGame = true;
        

    }

    public void StopGame() {
        waves.stopGame();

    }

    public void UnpauseGame() {
        Debug.Log("perk selected");
        pauseGame = false;

    }

    public void endGame() {
        gameOver = true;
        pauseGame = true;
        UIManager.ShowDeathUI();

    }

    public void spawnPerkOptions() {
        landmines.clearBombfield();
        showPerks = false;
        enemiesLeft.text = "Pick a new perk!";
        perkManager.SpawnPerkOption(0);
        perkManager.SpawnPerkOption(1);
        perkManager.SpawnPerkOption(2);

    }


    private void Update() {
        playerInMenu = mainMenu.viewingMainMenu() || gameOver;

        if (!playerInMenu) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

        } else {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;


        }
        


        if (waves.EnemiesLeft() == 0 && !waves.isSpawningEnemies() && startGame) {
            if (pauseDebounce) {
                pauseDebounce = false;
                pauseGame = true;
                showPerks = true;
            
            }

            if (pauseGame) {

                if (!perkManager.PlayerSelectedPerk()) {
                    waves.stopGame();

                }

                    
                 if (waveCompleteTimer < 1)
                 {
                     waveCompleteTimer += Time.deltaTime;
                     enemiesLeft.text = "Wave Complete";

                } else if (waveCompleteTimer >= 1) {
                    if (showPerks) {
                        spawnPerkOptions();

                    }
                }

            } else {
                if (waveCooldown < waveDelay && !waves.isSpawningEnemies()) {
                    landmines.toggleSpawningBombs(true);
                    enemiesLeft.text = "Wave starting in " + Mathf.CeilToInt(waveDelay - waveCooldown);
                    waveCooldown += Time.deltaTime;
                    playerHealth.setHealth(playerHealth.maxHealth);
                    intermission = true;
                    waveCompleteTimer = 0;

                } else if (waveCooldown >= waveDelay) {
                    landmines.toggleSpawningBombs(false);
                    intermission = false;
                    waveCooldown = 0;
                    waves.StartWave();
                    pauseDebounce = true;
                }
            }
        }
    }

}

