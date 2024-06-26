using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// pause the gme when they press P
public class PauseGame : MonoBehaviour {

    // serialize field stuff
    [SerializeField] Canvas pauseUI;
    [SerializeField] Canvas playerCanvas;
    [SerializeField] Tutorial tutorial;
    [SerializeField] ButtonManager buttonManager;

    private bool visible;

    // Start is called before the first frame update
    void Start() {
        pauseUI.enabled = false;

    }
    
    // just in case of some funny stuff
    private void OnDisable() {
        Time.timeScale = 1;
    }

    void Update() {

        // pause game if you aren't in the main menu
        if (!buttonManager.viewingMainMenu()) {

            // press p to pause game
            if (Input.GetKeyDown(KeyCode.P)) {
                visible = true;
                pauseGame();

            // hide pause menu UI, useful for taking ss
            } else if (Input.GetKeyDown(KeyCode.H)) {
                visible = !visible;
                pauseUI.enabled = visible;

            }
        } else {
            Time.timeScale = 1;
            visible = false;

        }
    }

    // let player pause game
    public void pauseGame() {
        Time.timeScale = 0; // pause game
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseUI.enabled = visible;
    }

    // let player unpause game
    public void UnpauseGame() {
        Time.timeScale = 1; // unpause game
        pauseUI.enabled = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }
}
