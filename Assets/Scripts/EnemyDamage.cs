using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float health;
    private float maxHealth;

    private float hitCooldown;
    private bool canTakeDamage = true;
    private bool tookDamage;
    private bool startCooldown;

    public Material FullHealthSprite;
    public Material MidHealthSprite;
    public Material LowHealthSprite;

    public GameObject enemySprite;

    public Rigidbody rb;

    private MeshRenderer spriteRenderer;

    private bool dead;
    private float ragdollTimer;
    public float lieOnFloorMaxTime;

    // Start is called before the first frame update
    void Start() {
        maxHealth = health;
        dead = false;
        spriteRenderer = enemySprite.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update() {

        if (health <= 0) {
            GetComponent<enemyAI>().enabled = false;
            dead = true;
        }

        if (health >= maxHealth * 0.75f) {
            spriteRenderer.material = FullHealthSprite;

        } else if (health >= maxHealth / 2) {
            spriteRenderer.material = MidHealthSprite;

        } else {
            spriteRenderer.material = LowHealthSprite;

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
        if (!dead) { 
            transform.position = -transform.forward + transform.position;
        }
    }

}
