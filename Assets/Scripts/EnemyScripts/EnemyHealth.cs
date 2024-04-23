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
    public Collider collider;

    public Rigidbody rb;

    private MeshRenderer spriteRenderer;

    private bool dead;
    private float ragdollTimer;
    public float lieOnFloorMaxTime;

    private bool appliedDeathForce;
    public WaveManager waveManager;

    // Start is called before the first frame update
    void Start() {
        appliedDeathForce = false;
        maxHealth = health;
        dead = false;
        spriteRenderer = enemySprite.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update() {

        if (health <= 0) {
            GetComponent<enemyAI>().enabled = false;
            
            collider.enabled = false;

            if (!appliedDeathForce) {
                rb.velocity = (new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10)));
                appliedDeathForce = true;
                waveManager.enemyDeath(this.gameObject);
            }

            if (ragdollTimer >= lieOnFloorMaxTime) {
                Destroy(gameObject);

            }
            else {
                ragdollTimer += Time.deltaTime;

            }

        }

        if (health >= maxHealth * 0.75f) {
            spriteRenderer.material = FullHealthSprite;

        } else if (health >= maxHealth / 2) {
            spriteRenderer.material = MidHealthSprite;

        } else {
            spriteRenderer.material = LowHealthSprite;

        }

    }

    public void takeDamage(float damage) {
        health -= damage;
        if (!dead) { 
            transform.position = -transform.forward + transform.position + new Vector3(0, 0, 0);
        }
    }

    public void addStatAmplifier(float newHealth) {
        health = newHealth;

    }

}