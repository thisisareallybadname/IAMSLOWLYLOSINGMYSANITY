using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public Image healthbar;
    public Image staminabar;

    private float health;
    public float maxHealth;

    private float stamina;
    public float maxStamina;

    private RectTransform healthTransform;
    private RectTransform staminaTransform;

    public PlayerDamage damageManager;
    public PlayerMovement playerMovement;

    public Camera mainCam;
    public Camera deathCam;

    private bool deathCamRotating;
    private float tick;

    private SpringModule recoilSpring;

    private bool toggleDeathUI;

    public Canvas playerUI;
    public Canvas deathUI;

    private bool deathDebounce;

    public Image crosshair;

    private float healthRatio; // ratio of current health / maximum health
    private float healthbarLost; // the amount of pixels the healthbar lost
    private float healthbarRemaining; // x width of healthbar

    private float staminaRatio;
    private float staminabarLost;
    private float staminabarRemaining;

    // Start is called before the first frame update
    void Start() {

        health = damageManager.getHealth();
        healthTransform = healthbar.GetComponent<RectTransform>();

        stamina = playerMovement.staminaValue;
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

        health = damageManager.getHealth();
        stamina = playerMovement.staminaValue;

        healthRatio = health / damageManager.maxHealth;

        healthbarRemaining = healthTransform.sizeDelta.x;
        healthbarLost = 240 - healthbarRemaining;

        stamina = playerMovement.staminaValue;
        staminaRatio = stamina / playerMovement.staminaLimit;

        staminabarRemaining = staminaTransform.sizeDelta.x;
        staminabarLost = 240 - staminabarRemaining;

        StartCoroutine(updateBar(healthTransform, healthbarLost, health / damageManager.maxHealth, 166.1f));
        StartCoroutine(updateBar(staminaTransform, staminabarLost, stamina / playerMovement.staminaLimit, 133.1f));

    }

    IEnumerator updateBar(RectTransform barTransform, float barLost, float ratio, float yOffset) {
        barTransform.sizeDelta = Vector2.Lerp(barTransform.sizeDelta, new Vector2(273.7f * ratio, 21.6f), 3 * Time.deltaTime);
        barTransform.anchoredPosition = Vector2.Lerp(barTransform.anchoredPosition, new Vector2(-267.5273f - barLost / 2, yOffset), 300 * Time.deltaTime);

        if (barTransform.anchoredPosition.x < -475) {
            barTransform.anchoredPosition = new Vector2(-475, barTransform.anchoredPosition.y);
        }

        yield return null;
    }

    public void ShowPlayerHUD() {
        mainCam.enabled = true;
        deathCam.enabled = false;

        mainCam.GetComponent<AudioListener>().enabled = true;
        deathCam.GetComponent<AudioListener>().enabled = false;

        deathCamRotating = false;

        playerUI.enabled = true;
        deathUI.enabled = false;
    }

    public void ShowDeathUI() {
        deathCamRotating = true;

        mainCam.enabled = false;
        deathCam.enabled = true;

        mainCam.GetComponent<AudioListener>().enabled = false;
        deathCam.GetComponent<AudioListener>().enabled = true;

        deathCam.transform.rotation = Quaternion.Euler(25, 1.5f * tick, 0);

        playerUI.enabled = false;
        deathUI.enabled = true;

    }
}
