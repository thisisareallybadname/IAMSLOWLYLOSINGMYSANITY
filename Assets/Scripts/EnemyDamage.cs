using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float health;
    private float hitCooldown;
    private bool canTakeDamage = true;
    private bool tookDamage;
    private bool startCooldown;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        
        if (health <= 0) {
            Destroy(this.gameObject);

        }

        
        

    }

    public void takeDamage(float damage) {
        health -= damage;
    }
}