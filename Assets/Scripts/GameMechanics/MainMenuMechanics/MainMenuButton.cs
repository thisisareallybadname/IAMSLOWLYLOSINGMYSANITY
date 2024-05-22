using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour {

    public Image title;
    public Button button;

    public Canvas playerUI;

    // Start is called before the first frame update
    void Start() {
        playerUI.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void help() {
        Debug.Log("i will eat your balls");
        title.enabled = false;
        button.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerUI.gameObject.SetActive(true);
    }
}
