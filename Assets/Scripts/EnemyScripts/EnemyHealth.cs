using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    [SerializeField] float health;
    private float maxHealth;

    public float kbResistance = 1;

    public float immunityDuration;
    private float hitCooldown = 999;

    private bool canTakeDamage = true;

    public Material FullHealthMeleeSprite; // melee enemy sprite when its health > 50
    public Material LowHealthMeleeSprite; // melee enemy sprite when its health < 50

    public Material FullHealthRangedSprite; // ranged enemy sprite when its health > 50
    public Material LowHealthRangedSprite; // ranged enemy sprite when its health < 50

     // default is melee enemy, changed to ranged enemy if specified
    private Material FullHealthEnemySprite;
    private Material LowHealthEnemySprite;

    public GameObject enemySprite;
    public Collider collider;

    private Rigidbody rb;

    private MeshRenderer spriteRenderer;

    private bool dead;
    private float ragdollTimer; // keeps track of how long ragdoll thingy is right now
    public float lieOnFloorMaxTime; // ragdoll length at death

    // debounce bool that makes it so while the enemy is doing that ragdoll thingy, it won't get moved
    private bool appliedDeathForce;
    public WaveManager waveManager;
    
    public enemyAI enemyAI; // disabled when health <= 0
    public Enemy enemyStats; // to be deleted

    public float enemyControl; // enemy drag, makes it less slippery and stupid
    public float knockbackControl; // makes less knockback

    public bool isPerkOption;
    public PerkManager perks;

    void Awake() {
        appliedDeathForce = false;
        maxHealth = health;
        dead = false;
        spriteRenderer = enemySprite.GetComponent<MeshRenderer>();
        rb = GetComponentRigidBody>();
        
        FullHealthEnemySprite = FullHealthMeleeSprite;
        LowHealthEnemySprite = LowHealthMeleeSprite;
        
    }

    // change sprites
    void EnableRangedAttack() {
        FullHealthEnemySprite = FullHealthRangedSprite;
        LowHealthEnemySprite = LowHealthRangedSprite;

    }

    // Update is called once per frame
    void Update() {
        if (health <= 0) {
            GetComponent<enemyAI>().enabled = false; // disable enemy AI because it's dead
            
            // if you didn't already, apply death force to enemy
            if (!appliedDeathForce) {
                rb.velocity = (new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10)));
                appliedDeathForce = true;
                waveManager.enemyDeath(this.gameObject);
            }

            // destroy enemy if it ragdolled too long
            if (ragdollTimer >= lieOnFloorMaxTime) {
                Destroy(gameObject);

            } else {
                ragdollTimer += Time.deltaTime;

            }

        }

        // change sprite depending on enemy's health
        if (health >= maxHealth /2f) {
            spriteRenderer.material = FullHealthEnemySprite;

        } else {
            spriteRenderer.material = LowHealthEnemySprite;

        } 

    }

    private void FixedUpdate() {

        // this makes knockback more dramatic????
        if (hitCooldown < immunityDuration) {
            hitCooldown += Time.fixedDeltaTime;
            rb.drag = knockbackControl;
            enemyAI.enabled = false;

        } else if (hitCooldown >= immunityDuration) {
            rb.drag = enemyControl;
            enemyAI.enabled = true;

        }
    }

    // change the sprites
    public void changeSprite(Material newSprite) {
        FullHealthEnemySprite = newSprite;
        LowHealthEnemySprite = newSprite;

    }

    // take damage (+ knockback if provided_ 
    public void takeDamage(float damage, float knockback) {
        // make sure that if it can't take kb when its dead
        if (!dead) {

            // since the perkOption enemy uses this as well (perk enemy), if the enemy is a perk option, instakill it
            if (isPerkOption) {
                health = 0;
                perks.PerkSelected(GetComponent<Enemy>());

            }

            // if the enemyAI isn't deleted yet, add knockback
            if (enemyStats.active) {
                hitCooldown = 0;

                // add knockback force to player
                rb.AddForce(new Vector3(0, 1, 0) * knockback * 50 + new Vector3(Random.Range(-knockback * 0.5f, knockback * 0.5f), 0, Random.Range(-knockback * 0.5f, knockback * 0.5f)) * (1 / kbResistance), ForceMode.Force);
                rb.velocity = new Vector3(Mathf.Abs(rb.velocity.x), Mathf.Abs(rb.velocity.y), Mathf.Abs(rb.velocity.z));
                enemyAI.enabled = false;
            }
        }

        
        health -= damage;
        
    }

    // change health (used w/ perkManager)
    public void addStatAmplifier(float newHealth) {
        health = newHealth;

    }

}
