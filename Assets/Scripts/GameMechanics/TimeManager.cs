using System.Collections;
using System.Collections.Generic;
using System.Security;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// time manager, makes the scripts run on time
public class TimeManager : MonoBehaviour {

    // public variables
    [SerializeField] WaveManager waves; // wave manager
    [SerializeField] PerkManager perkManager; // perk manager, current script tells this to summmon perk options, and this one tells current script when player did select a perk 
    [SerializeField] LandmineSetter landmines; // creates landmine field
    [SerializeField] PlayerDamage playerHealth; // player's health, heals it to max during intermission phase 
    [SerializeField] ButtonManager mainMenu;
    [SerializeField] UIManager UIManager;

    private bool startGame; // true if tutorial has been finished
    private bool gameOver; // true if player is at 0 hp
    
    [SerializeField] bool spawnBombs;
    private bool pauseDebounce;

    [SerializeField] GameObject originalEnemy;
    private EnemyMovement originalEnemyMovement;
    private EnemyHealth originalEnemyHealth;

    public bool pauseGame; // true if game is paused (obviously) | enabled when selecting perk 
    private bool playerInMenu; // true if player is in main menu/game over screen

    // used to keep track of time in intermission phase
    private float waveCooldown; // timer thingy
    
    private float waveDelay = 5; // length of intermission waiting phase
    [SerializeField] bool intermission; // true if ongoing intermission phase (more on that later) 

    private float waveCompleteTimer; // more on this later
    private bool spawnedInPerkOptions = false; // true if perks are showing

    [SerializeField] TMP_Text enemiesLeft; // thingy in player hud that shows how much enemies are left
    [SerializeField] TMP_Text waveCounter;
    [SerializeField] Image enemiesLeftBG;

    [SerializeField] Image waveCounterBG;
    [SerializeField] GameObject enemySpawnIndicator;

    private bool clearedMinefield;
    private bool spawnedLandmines;
    private bool spawnedLandmineIndicators;

    // starts a new game, resets stats
    public void StartGame() {
        pauseDebounce = true;
        startGame = true;

        enemiesLeftBG.enabled = true;
        waveCounterBG.enabled = true;
        enemiesLeft.enabled = true;
        waveCounter.enabled = true;

        originalEnemyMovement = originalEnemy.GetComponent<EnemyMovement>();
        originalEnemyHealth = originalEnemy.GetComponent<EnemyHealth>();

    }

    // unpauses game, used when player selects perk
    public void UnpauseGame() {
        pauseGame = false;

    }

    // ends the game, activated by playerDamage script when health is 0
    public void endGame() {
        gameOver = true;
        pauseGame = true;
        startGame = false;
        UIManager.ShowDeathUI();

    }

    // !!! [pretty important] bulk of the perk-selecting phase
    // summons all the perk options avalable
    public void spawnPerkOptions() {
        landmines.clearBombfield();

        // tell player to change a perk
        enemiesLeft.text = "Pick a new perk!";
        perkManager.SpawnPerkOption(0);
        perkManager.SpawnPerkOption(1);
        perkManager.SpawnPerkOption(2);

    }

    private void ManageIntermissionPhase() {
        if (waveCooldown < waveDelay && !waves.isSpawningEnemies()) {
            // update "Enemies Left" ui
            enemiesLeft.text = "Wave starting in " + Mathf.CeilToInt(waveDelay - waveCooldown);
            if (waveCounter.text.Equals("Tutorial")) {
                waveCounter.text = "Wave 0";

            }

            if (!spawnedLandmineIndicators) {
                spawnedLandmineIndicators = true;
                landmines.createLandmineSpawns();
            }
            waveCompleteTimer = 0;

            enemySpawnIndicator.GetComponent<MeshRenderer>().enabled = true;

            playerHealth.heal(1);

            // thing that keeps track of time
            waveCooldown += Time.deltaTime;

            // wave finished setting up, unpause game and begin new wave
            // reset pauseDebounce variable so that next intermission period it will trigger

        } else if (waveCooldown >= waveDelay && !pauseDebounce) {
            if (!spawnedLandmines) {
                landmines.placeLandmines(); // spawn landmine indicators
                spawnedLandmines = true;
            }
            
            // prepare stuff for enemy
            waveCooldown = 0;
            waves.StartWave();
            
            pauseDebounce = true;

            // reset vars
            clearedMinefield = false;
            spawnedLandmines = false;
            spawnedLandmineIndicators = false;

            intermission = false;

            enemySpawnIndicator.GetComponent<MeshRenderer>().enabled = false;

        }

    }

    public bool GameInIntermissionPhase() {
        return intermission || spawnedInPerkOptions || !startGame ;

    }

    // force quit intermission or perk
    public void QuitIntermissionPeriod() {
        intermission = false;
        pauseGame = true;
        waveCooldown = 0;

    }

    // update method
    private void Update() {

        // intermission phase
        
        // intermission phase is when:
        // - no enemies present
        // - no enemies are getting spawned in 
        // - player finished/skipped tutorial
        
        // two parts to intermission phase, 
        // - perk-selecting part, part where player is selecting a perk
        // - "cooldown" part, it's a five second wait time between perk-selecting part and the end of the intermission period
        
        if (waves.EnemiesLeft() == 0 && !waves.isSpawningEnemies() && startGame) {

            if (!clearedMinefield){
                clearedMinefield = true;
                landmines.clearBombfield();

            }

            //  !!! [pretty important] pauses the game

            // debounce variable so that pauseGame won't always be spammed every time the criteria above are met
            // if there was no debounce/cooldown variable, the game will be indefinitely paused
            if (pauseDebounce) {
                pauseDebounce = false;
                pauseGame = true;
                spawnedInPerkOptions = false;
            
            }

            //-- the stuff that makes intermission phase work --\\
            if (pauseGame) {

                // keep the player alive at all costs because if they die in intermission phase it breaks a lot of stuff
                playerHealth.heal(1);

                // pause the game 
                if (!perkManager.PlayerSelectedPerk()) {
                    intermission = true;
                    waves.stopGame();
                }

                //-- little thing that makes the wave UI say "Wave Completed!" when you beat a wave for about a second
                 if (waveCompleteTimer < 1) {
                    if (waves.getWave() > 0)
                    {
                        waveCompleteTimer += Time.deltaTime;
                        enemiesLeft.text = "Wave Completed!";

                    } else {
                        waveCompleteTimer = 2f;
                        enemiesLeft.text = "Pick a Perk!";

                    }
                 // !!! [pretty important] perk-selection phase
                 } else if (waveCompleteTimer >= 1) {
                    if (!spawnedInPerkOptions) {
                        spawnedInPerkOptions = true;

                        // buff enemies a little bit, it's in this method because 
                        // 1. it will only run once (hopefully)
                        // 2. it's placed right before the stats get modified again
                        originalEnemyMovement.setWalkspeed(waves.getWave() / 8.75f, "add");
                        originalEnemyHealth.setHealth(waves.getWave() + 1, "set");

                        spawnPerkOptions(); // most of the perk-selecting phase code is in here

                    }
                }

            } else {
                ManageIntermissionPhase();

            }
        }
    }

}

