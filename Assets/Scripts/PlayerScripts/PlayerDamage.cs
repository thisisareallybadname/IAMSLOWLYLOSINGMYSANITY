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
    private bool takeDamage;

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


    private void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.tag.Equals("Enemy")) {
            if (canTakeDamage) {
                health -= 1;
                canTakeDamage = false;
                hurtEffect.applyCameraForce(new Vector3(0, 0, -22.5f), new Vector3(0, 0.75f, -0.5f));

            }

        }

    }

    public float getHealth() {
        return health;
    }

    public void setHealth(float newHealth) { 
        health = newHealth; 
    }

}
