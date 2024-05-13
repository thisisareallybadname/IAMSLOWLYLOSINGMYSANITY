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

    // Start is called before the first frame update
    void Start() {
        canTakeDamage = false;
        controller = GetComponent<CharacterController>();
        
    }

    // Update is called once per frame
    void Update() {

        if (transform.position.y < -20) {
            setHealth(0);

        }

        if (!canTakeDamage) {
            immunityTimer += Time.deltaTime;

        }

        if (immunityTimer >= immunityLength)
        {
            canTakeDamage = true;
            immunityTimer = 0;
        }        
    }


    private void OnTriggerStay(Collider collision) {
        if (collision.gameObject.tag.Equals("Enemy") && collision.gameObject.GetComponent<Enemy>().active) {
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
}
