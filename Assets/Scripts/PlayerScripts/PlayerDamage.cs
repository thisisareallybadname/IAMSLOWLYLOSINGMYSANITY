using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PlayerDamage : MonoBehaviour {
    private CharacterController controller;
        
    private float knockback;
    public float knockbackValue;

    private float knockbackDuration;

    public float health;

    private float immunityTimer;
    public float immunityLength;
    private bool canTakeDamage;

    public PlayerCamera hurtEffect;
    public TimeManager timeManager;
    public UIManager uiManager;

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

        if (immunityTimer >= immunityLength)
        {
            canTakeDamage = true;
            immunityTimer = 0;
        }
        
        if (health <= 0) {
            health = 0;
            if (deathDebounce) {
                deathDebounce = false;
                timeManager.endGame();

            }
        } else {
            deathDebounce = true;

        }

    }


    private void OnTriggerStay(Collider collision) {
        if (collision.gameObject.tag.Equals("Enemy") && collision.gameObject.GetComponent<EnemyHealth>().getHealth() > 0) {
            takeDamage(collision.gameObject.GetComponent<Enemy>().getDamage());

        }

    }

    public float getHealth() {
        return health;
    }

    public void setHealth(float newHealth) { 
        health = newHealth; 

    }

    public void takeDamage(float damage) {
        if (canTakeDamage) {
            canTakeDamage = false;
            hurtEffect.applyCameraForce(new Vector3(0, 0, -22.5f), new Vector3(0, 0.75f, -0.5f));
            health -= damage;

        }

    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag.Equals("DEATH")) {
            takeDamage(999999999999999999);

        }
        
    }

}
