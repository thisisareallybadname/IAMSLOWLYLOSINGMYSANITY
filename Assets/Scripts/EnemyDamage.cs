using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float health;
    private float hitCooldown;
    private bool canTakeDamage = true;
    private bool tookDamage;
    private bool startCooldown;

    private bool dead;
    private float ragdollTimer;
    public float lieOnFloorMaxTime;

    // Start is called before the first frame update
    void Start()
    {
        dead = false;
    }

    // Update is called once per frame
    void Update() {
        
        if (health <= 0) {
            GetComponent<enemyAI>().enabled = false;
            dead = true;
        }

        if (dead) {
            if (ragdollTimer >= lieOnFloorMaxTime) {
                Destroy(gameObject);

            } else {
                ragdollTimer += Time.deltaTime;

            }

        }
        

    }

    public void takeDamage(float damage) {
        health -= damage;
    }

}
