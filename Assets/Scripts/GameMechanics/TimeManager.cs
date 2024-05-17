using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {

    public WaveManager waves;
    public Tutorial tutorial;
    public PerkManager perkManager;
    public LandmineSetter landmines;
    
    private bool gameOver;
    public bool spawnBombs;

    // Start is called before the first frame update
    public void PauseGame() {
        waves.stopGame();

    }

    public void StartGame() {
        waves.startGame();

    }

    public void StopGame() {
        waves.stopGame();

    }

    // Update is called once per frame
    private void Update() {
        landmines.canSpawnBombs = spawnBombs;

        if (waves.GamePaused()) {
            waves.stopGame();

            if (!perkManager.selectedPerk) {
                perkManager.SpawnPerkOption();

            }


        } else {
        }

    }
}
