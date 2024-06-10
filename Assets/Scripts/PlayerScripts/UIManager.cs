using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

// manages player HUD and death HUD
public class UIManager : MonoBehaviour {
    
    [SerializeField] Image healthbar;
    [SerializeField] Image staminabar; 

    private float health; // player's max health
    [SerializeField] float maxHealth;

    private float stamina; // player's max stamina
    [SerializeField] float staminaLimit; 
    
    private RectTransform healthTransform; // the size vector of healthbar
    private RectTransform staminaTransform; // size vector of stamina bar

    public PlayerDamage damageManager; // used to keep constantly update health var
    public PlayerMovement playerMovement; // used to constantly update stamina bar

    [SerializeField] Camera mainCam; // player camera
    [SerializeField] Camera deathCam; // spinning camera in "YOu DIED!!!!!" screen
    [SerializeField] Camera VMcam;

    private bool deathCamRotating; 
    private float tick; // used for moving death camera

    private bool toggleDeathUI;

    [SerializeField] Canvas playerUI; // player HUD (healthbar + staminabar, wave counter, etc.)
    [SerializeField] Canvas deathUI; // you died + respawn button

    private float healthRatio; // ratio of current health / maximum health
    private float healthbarLost; // how much pixels healthbar shrank

    private float staminaRatio; // stamina / maxStamina
    private float staminabarLost; // same thing as healthbarLost but for stamina

    // set up variables
    void Start() {
        health = damageManager.getHealth();
        healthTransform = healthbar.GetComponent<RectTransform>();

        stamina = playerMovement.GetStaminaLimit();
        staminaTransform = staminabar.GetComponent<RectTransform>();

        mainCam.enabled = true;
        deathCam.enabled = false;

        mainCam.GetComponent<AudioListener>().enabled = true;
        deathCam.GetComponent<AudioListener>().enabled = false;

        playerUI.enabled = true;
        deathUI.enabled = false;



    }

    // Update is called once per frame
    void Update() {
        if (deathCamRotating) {
            tick += Time.deltaTime;

        }

        // constantly update health & stamina variables
        stamina = playerMovement.getStaminaValue();
        staminaLimit = playerMovement.GetStaminaLimit();

        health = damageManager.getHealth();

        // calculate ratios for bars
        healthRatio = health / damageManager.getMaxHealth();
        healthbarLost = 240 - healthTransform.sizeDelta.x;

        stamina = playerMovement.getStaminaValue();
        staminaRatio = stamina / staminaLimit;

        staminabarLost = 240 - staminaTransform.sizeDelta.x;

        // update both bars
        StartCoroutine(updateBar(healthTransform, healthbarLost, health / damageManager.maxHealth, 166.1f));
        StartCoroutine(updateBar(staminaTransform, staminabarLost, stamina / staminaLimit, 133.1f));

    }

    // lerp bar's position and size
    IEnumerator updateBar(RectTransform barTransform, float barLost, float ratio, float yOffset) {

        barTransform.sizeDelta = Vector2.Lerp(barTransform.sizeDelta, new Vector2(258f * ratio, 21.6f), 3 * Time.deltaTime);
        if (barTransform.sizeDelta.x > 258) {
            barTransform.sizeDelta = new Vector2(258, 21.6f);

        }
        // lerp value for position is so high compared to its size because it actually does its job really well and finishes first
        barTransform.anchoredPosition = Vector2.Lerp(barTransform.anchoredPosition, new Vector2(-267.5273f - barLost / 2, yOffset), 300 * Time.deltaTime);

        if (barTransform.anchoredPosition.x < -475) {
            barTransform.anchoredPosition = new Vector2(-475, barTransform.anchoredPosition.y);
        }

        yield return null;
    }

    // shows the player hud, disables death ui
    public void ShowPlayerHUD() {
        mainCam.enabled = true;
        deathCam.enabled = false;

        mainCam.GetComponent<AudioListener>().enabled = true;
        deathCam.GetComponent<AudioListener>().enabled = false;

        deathCamRotating = false;

        playerUI.enabled = true;
        deathUI.enabled = false;
    }

    // disables hud, shows death ui stuff
    public void ShowDeathUI() {
        deathCamRotating = true;

        mainCam.enabled = false;
        deathCam.enabled = true;
        VMcam.enabled = false;

        mainCam.GetComponent<AudioListener>().enabled = false;
        deathCam.GetComponent<AudioListener>().enabled = true;

        deathCam.transform.rotation = Quaternion.Euler(25, 1.5f * tick, 0);

        playerUI.enabled = false;
        deathUI.enabled = true;

    }
}
