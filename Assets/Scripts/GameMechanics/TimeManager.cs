using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {

    public WaveManager waves;
    public Tutorial tutorial;

    // Start is called before the first frame update
    public void PauseGame() {
        waves.startGame = false;

    }

    public void StartGame() {
        waves.resetGame();
        waves.startGame = true;

    }

    // Update is called once per frame
    void EndGame() {
        waves.startGame = false;

    }
}
