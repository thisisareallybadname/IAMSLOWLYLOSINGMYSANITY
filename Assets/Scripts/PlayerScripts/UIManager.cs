using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public Image healthbar;

    private float health;
    public float maxHealth;
    private RectTransform healthTransform;
    public PlayerDamage damageManager;

    public Camera mainCam;
    public Camera deathCam;

    private float tick;

    private SpringModule recoilSpring;

    private bool toggleDeathUI;

    public Canvas playerUI;
    public Canvas deathUI;

    public Image crosshair;

    private float healthRatio; // ratio of current health / maximum health
    private float healthbarLost; // the amount of pixels the healthbar lost
    private float healthbarRemaining; // x width of healthbar

    // Start is called before the first frame update
    void Start() {

        health = damageManager.getHealth();
        healthTransform = healthbar.GetComponent<RectTransform>();

        mainCam.enabled = true;
        deathCam.enabled = false;

        mainCam.GetComponent<AudioListener>().enabled = true;
        deathCam.GetComponent<AudioListener>().enabled = false;

        playerUI.enabled = true;
        deathUI.enabled = false;

    }

    // Update is called once per frame
    void Update() {
        health = damageManager.getHealth();

        healthRatio = health / maxHealth;

        healthbarRemaining = healthTransform.sizeDelta.x;
        healthbarLost = 240 - healthbarRemaining;

        StartCoroutine(updateBar(healthTransform));

        if (health <= 0) {
            tick += Time.deltaTime;

            mainCam.enabled = false;
            deathCam.enabled = true;

            mainCam.GetComponent<AudioListener>().enabled = false;
            deathCam.GetComponent<AudioListener>().enabled = true;

            deathCam.transform.rotation = Quaternion.Euler(25, 1.5f * tick, 0);

            playerUI.enabled = false;
            deathUI.enabled = true;
        }
    }

    IEnumerator updateBar(RectTransform barTransform) {
        barTransform.sizeDelta = Vector2.Lerp(healthTransform.sizeDelta, new Vector2(240 * healthRatio, 36), 3 * Time.deltaTime);
        barTransform.anchoredPosition = Vector2.Lerp(healthTransform.anchoredPosition, new Vector2(-healthbarLost / 2 - 9, -205), 12 * Time.deltaTime);

        yield return null;
    }

    public void expandCrosshair(float size) {
    }
}
