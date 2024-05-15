using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class TimeManager : MonoBehaviour {

    public WaveManager waves;
    public PerkManager perkManager;
    public Tutorial tutorial;
    public bool selectedOption;

    // Start is called before the first frame update
    public void PauseGame() {
        waves.running = false;

    }

    public void StartGame() {
        waves.resetGame();
        waves.running = true;

    }

    // Update is called once per frame
    void EndGame() {
        waves.running = false;

    }

    private void Update() {
        if (perkManager.selectedOption) {
            waves.frog = true;

        }

        if (false) {
            waves.frog = false;

        }
    }
}
