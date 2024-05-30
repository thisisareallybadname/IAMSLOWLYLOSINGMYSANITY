using System.Collections;
using System.Collections.Generic;
using System.Security;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

// time manager, 
public class TimeManager : MonoBehaviour {

    // public variables
    public WaveManager waves; // wave manager
    public PerkManager perkManager; // perk manager, current script tells this to summmon perk options, and this one tells current script when player did select a perk 
    public LandmineSetter landmines; // creates landmine field
    public PlayerDamage playerHealth; // player's health, heals it to max during intermission phase 
    public MainMenuButton mainMenu;
    public UIManager UIManager;

    private bool startGame; // true if tutorial has been finished
    private bool gameOver; // true if player is at 0 hp
    
    public bool spawnBombs;
    private bool pauseDebounce;
    
    public bool pauseGame; // true if game is paused (obviously) | enabled when selecting perk 
    private bool playerInMenu; // true if player is in main menu/game over screen

    // used to keep track of time in intermission phase
    private float waveCooldown; // timer thingy
    
    private float waveDelay = 5; // length of intermission waiting phase
    public bool intermission; // true if ongoing intermission phase (more on that later) 

    private float waveCompleteTimer; // more on this later
    private bool showPerks = false; // true if perks are showing

    public TMP_Text enemiesLeft; // thingy in player hud that shows how much enemies are left

    // pauses the game, maybe unused?????
    public void PauseGame() {
        waves.stopGame();

    }

    // starts a new game, resets stats
    public void StartGame() {
        Debug.Log("tutorial finished");
        pauseDebounce = true;
        startGame = true;
        

    }

    // stops the game, probably unused
    public void StopGame() {
        waves.stopGame();

    }

    // unpauses game, used when player selects perk
    public void UnpauseGame() {
        Debug.Log("perk selected");
        pauseGame = false;

    }

    // ends the game, activated by playerDamage script when health is 0
    public void endGame() {
        gameOver = true;
        pauseGame = true;
        UIManager.ShowDeathUI();

    }

    // !!! [pretty important] bulk of the perk-selecting phase
    // summons all the perk options avalable
    public void spawnPerkOptions() {
        landmines.clearBombfield();
        showPerks = false;

        // tell player to change a perk
        enemiesLeft.text = "Pick a new perk!";
        perkManager.SpawnPerkOption(0);
        perkManager.SpawnPerkOption(1);
        perkManager.SpawnPerkOption(2);

    }

    // update method
    private void Update() {

        // checks if the player is playing the game (fighting through waves and stuff)
        // if they are doing this, lock their cursor onto the screen
        // if they aren't, unlock it
        playerInMenu = mainMenu.viewingMainMenu() || gameOver;
        
        if (!playerInMenu) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

        } else {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;


        }

        // intermission phase
        
        // intermission phase is when:
        // - no enemies present
        // - no enemies are getting spawned in 
        // - player finished/skipped tutorial
        
        // two parts to intermission phase, 
        // - perk-selecting part, part where player is selecting a perk
        // - "cooldown" part, it's a five second wait time between perk-selecting part and the end of the intermission period
        
        if (waves.EnemiesLeft() == 0 && !waves.isSpawningEnemies() && startGame) {

            //  !!! [pretty important] pauses the game
            
            // debounce variable so that pauseGame won't always be spammed every time the criteria above are met
            // if there was no debounce/cooldown variable, the game will be indefinitely paused
            if (pauseDebounce) {
                pauseDebounce = false;
                pauseGame = true;
                showPerks = true;
            
            }

            //-- the stuff that makes intermission phase work --\\
            if (pauseGame) {
                
                // pause the game 
                if (!perkManager.PlayerSelectedPerk()) {
                    waves.stopGame(); ///// !!!!!!! [VERY IMPORTANT!!!] move this line of code out of this if statement because i think its pretty redundant

                }

                //-- little thing that makes the wave UI say "Wave Completed!" when you beat a wave for about a second
                 if (waveCompleteTimer < 1) {
                     waveCompleteTimer += Time.deltaTime;
                     enemiesLeft.text = "Wave Completed!";

                 // !!! [pretty important] perk-selection phase
                 } else if (waveCompleteTimer >= 1) {
                    if (showPerks) {
                        showPerks = false;
                        spawnPerkOptions(); // most of the perk-selecting phase code is in here

                    }
                }

            // !!!!! [VERY important] code for waiting-phase of intermission
            } else {

                playerHealth.setHealth(playerHealth.maxHealth); // keep the player alive at all costs because if they die in intermission phase it breaks a lot of stuff

                // keep track of time spent in intermission phase + begin to set up map
                if (waveCooldown < waveDelay && !waves.isSpawningEnemies()) {
                    playerHealth.setHealth(playerHealth.maxHealth); // keep the player alive at all costs because if they die in intermission phase it breaks a lot of stuff
                    landmines.toggleSpawningBombs(true); // let landmine indicators spawn in
                    enemiesLeft.text = "Wave starting in " + Mathf.CeilToInt(waveDelay - waveCooldown); // update UI
                    
                    intermission = true;
                    waveCompleteTimer = 0;

                    // thing that keeps track of time
                    waveCooldown += Time.deltaTime;

                // wave finished setting up, unpause game and begin new wave
                // reset pauseDebounce variable so that next intermission period it will trigger
                    
                } else if (waveCooldown >= waveDelay) {
                    landmines.toggleSpawningBombs(false); // spawn landmine indicators
                    playerHealth.setHealth(playerHealth.maxHealth); // keep the player alive at all costs because if they die in intermission phase it breaks a lot of stuff
                    intermission = false;
                    waveCooldown = 0;
                    waves.StartWave();
                    pauseDebounce = true;
                }
            }
        }
    }

}

