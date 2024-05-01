using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    public float health;
    private float maxHealth;

    public float immunityDuration;
    private float hitCooldown = 999;

    private bool canTakeDamage = true;

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

    public enemyAI enemyAI;

    public float enemyControl;
    public float knockbackControl;

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

    private void FixedUpdate() {

        if (hitCooldown < immunityDuration) {
            hitCooldown += Time.fixedDeltaTime;
            rb.drag = knockbackControl;
            enemyAI.enabled = false;

        } else if (hitCooldown >= immunityDuration) {
            rb.drag = enemyControl;
            enemyAI.enabled = true;

        }
    }

    public void takeDamage(float damage, float knockback) {
        if (!dead) {
            if (enemyAI.chasePlayer) {
                hitCooldown = 0;
                rb.velocity = new Vector3(Mathf.Abs(rb.velocity.x), Mathf.Abs(rb.velocity.y), Mathf.Abs(rb.velocity.z));
                rb.velocity = -rb.transform.forward * knockback;
                enemyAI.enabled = false;
            }
        }

        health -= damage;
        
    }

    public void addStatAmplifier(float newHealth) {
        health = newHealth;

    }

}
