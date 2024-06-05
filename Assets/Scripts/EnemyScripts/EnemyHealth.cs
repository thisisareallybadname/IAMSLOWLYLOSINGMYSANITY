using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    public float health;
    private float maxHealth;

    public float kbResistance = 1;

    public float immunityDuration;
    private float hitCooldown = 999;

    private bool canTakeDamage = true;

    public Material FullHealthMeleeSprite;
    public Material LowHealthMeleeSprite;

    public Material FullHealthRangedSprite;
    public Material LowHealthRangedSprite;

    private Material FullHealthEnemySprite;
    private Material LowHealthEnemySprite;

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
    public Enemy enemyStats;

    public float enemyControl;
    public float knockbackControl;

    public bool isPerkOption;
    public PerkManager perks;

    // Start is called before the first frame update
    void Awake() {
        appliedDeathForce = false;
        maxHealth = health;
        dead = false;
        spriteRenderer = enemySprite.GetComponent<MeshRenderer>();

        FullHealthEnemySprite = FullHealthMeleeSprite;
        LowHealthEnemySprite = LowHealthMeleeSprite;
        
    }

    void EnableRangedAttack() {
        FullHealthEnemySprite = FullHealthRangedSprite;
        LowHealthEnemySprite = LowHealthRangedSprite;

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
            spriteRenderer.material = FullHealthEnemySprite;

        } else if (health >= maxHealth / 2) {
            spriteRenderer.material = LowHealthEnemySprite;

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

    public void changeSprite(Material newSprite) {
        FullHealthEnemySprite = newSprite;
        LowHealthEnemySprite = newSprite;

    }

    public void takeDamage(float damage, float knockback) {
        if (!dead) {
            if (isPerkOption) {
                health = 0;
                perks.PerkSelected(GetComponent<Enemy>());

            }

            if (enemyStats.active) {
                hitCooldown = 0;
                rb.velocity = new Vector3(Mathf.Abs(rb.velocity.x), Mathf.Abs(rb.velocity.y), Mathf.Abs(rb.velocity.z));
                enemyAI.enabled = false;
            }
        }

        rb.AddForce(new Vector3(0, 1, 0) * knockback * 50 + new Vector3(Random.Range(-knockback * 0.5f, knockback * 0.5f), 0, Random.Range(-knockback * 0.5f, knockback * 0.5f)) * (1 / kbResistance), ForceMode.Force);

        health -= damage;
        
    }

    public void addStatAmplifier(float newHealth) {
        health = newHealth;

    }

}
