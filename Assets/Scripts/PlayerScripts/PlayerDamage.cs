using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

// manages damage intake player gets
public class PlayerDamage : MonoBehaviour {
    private CharacterController controller;
        
    private float knockback;
    public float knockbackValue;

    private float knockbackDuration;

    public float health;

    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] FireWeapon leftWeapon;
    [SerializeField] FireWeapon rightWeapon;
    [SerializeField] PauseGame pauseManagerThingy;

    // doctors' recommended damage intake is 1 dmg / 1 sec
    
    // immunity timers 
    private float immunityTimer;
    public float immunityLength;
    private bool canTakeDamage;

    // shove player camera every time player takes damage thru playerCamera
    public PlayerCamera hurtEffect;
    public TimeManager timeManager;
    [SerializeField] WaveManager waveManager;
    public UIManager uiManager;

    // make sure that timaManager doesn't get spammed w/ stopGame()
    private bool deathDebounce;

    public float maxHealth;

    // Start is called before the first frame update
    void Start() {
        health = maxHealth;
        canTakeDamage = false;
        controller = GetComponent<CharacterController>();
        
    }

    // Update is called once per frame
    void Update() {
        if (!canTakeDamage) {
            immunityTimer += Time.deltaTime;

        }
        if (immunityTimer >= immunityLength) {
            canTakeDamage = true;
            immunityTimer = 0;
        }

        // make cursor work normally again when health = 0``
        if (health <= 0) {
            health = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // end the game when player dead
            if (deathDebounce) {
                timeManager.endGame();
                waveManager.clearWave();
                pauseManagerThingy.enabled = false;
            }
        } else {

            // reset debounce var when player isn't dead
            if (!deathDebounce) {
                deathDebounce = true;
            
            }
        }

    }

    // if player is touching an enemy and if they have no iframes, make them take damage
    private void OnTriggerStay(Collider collision) {
        if (collision.gameObject.tag.Equals("Enemy") && 
            collision.gameObject.GetComponent<EnemyHealth>().getHealth() > 0) {
            takeDamage(1);
            
        }
    }

    // get player's health, used in player UI script
    public float getHealth() {
        return health;
    }

    // too lazy to do some complicated stuff this is really convient to make
    // percentage here is 1 / value wanted
    public void heal(float percentage) {
        health = percentage * maxHealth;

    }

    // set max health, with three modes, add, mutl, and set
    public void setMaxHealth(float newHealth, string mode) {
        if (mode.Equals("add")) {
            maxHealth += newHealth;

        } else if (mode.Contains("mult")) { //contains because im very inconsistent in typing thingy
            maxHealth *= newHealth;

        } else {
            maxHealth = newHealth;

        }

        health = maxHealth;
    }

    // get max health, used in player UI script
    public float getMaxHealth() {
        return maxHealth;

    }

    // reset stats (what did you expect?????)
    public void resetStats() {
        maxHealth = 5;

    }

    // take damage if theres no iframes
    public void takeDamage(float damage) {
        if (canTakeDamage) {
            canTakeDamage = false;

            // camera hurt effect
            hurtEffect.applyCameraForce(new Vector3(0, 0, -0.75f), Quaternion.Euler(0, 1.75f, -0.5f));
            health -= damage;

        }

    }

    // set health
    public void setHealth(float newHealth) {
        health = newHealth;

    }
}
