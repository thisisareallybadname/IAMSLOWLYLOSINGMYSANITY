using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public VMEffects leftBobEffect;
    public VMEffects rightBobEffect;
    private Vector3 playerBob;
    public PlayerHealth playerHealth;

    public Image healthbar;
    public Image hud;
    public Image deathScreen;

    private float health;
    private float maxHealth;

    RectTransform healthbarPos;

    // Start is called before the first frame update
    void Start() {
        healthbarPos = healthbar.GetComponent<RectTransform>();
        health = playerHealth.getHealth();
        maxHealth = health;
        deathScreen.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -55);
        healthbarPos.anchoredPosition = new Vector2(0, -50);
        healthbarPos.sizeDelta = new Vector2(200 * (health / maxHealth), 30);
    }

    public void PlrTakeDamage(float damage) {
        health -= damage;
        //healthbarPos.sizeDelta -= new Vector2(maxHealth /  health, 1);
        
    }

    public void die() {
        Destroy(healthbar);
        Destroy(hud);
        deathScreen.enabled = true;



    }


}
