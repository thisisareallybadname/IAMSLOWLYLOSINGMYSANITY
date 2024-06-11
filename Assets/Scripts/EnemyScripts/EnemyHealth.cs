using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// manages enemy's health
public class EnemyHealth : MonoBehaviour {

    [SerializeField] float health;
    private float maxHealth;

    [SerializeField] float kbResistance = 1;

    [SerializeField] float immunityDuration;
    private float hitCooldown = 999;

    [SerializeField] Material FullHealthMeleeSprite; // melee enemy sprite when its health > 50
    [SerializeField] Material LowHealthMeleeSprite; // melee enemy sprite when its health < 50

    [SerializeField] Material FullHealthRangedSprite; // ranged enemy sprite when its health > 50
    [SerializeField] Material LowHealthRangedSprite; // ranged enemy sprite when its health < 50

     // default is melee enemy, changed to ranged enemy if specified
    private Material FullHealthEnemySprite;
    private Material LowHealthEnemySprite;

    [SerializeField] GameObject enemySprite;

    private Rigidbody rb;

    private MeshRenderer spriteRenderer;

    private bool dead;
    private float ragdollTimer; // keeps track of how long ragdoll thingy is right now
    [SerializeField] float lieOnFloorMaxTime; // ragdoll length at death

    // debounce bool that makes it so while the enemy is doing that ragdoll thingy, it won't get moved
    private bool appliedDeathForce;
    public WaveManager waveManager;
    
    [SerializeField] EnemyMovement enemyAI; // disabled when health <= 0

    [SerializeField] float enemyControl; // enemy drag, makes it less slippery and stupid
    [SerializeField] float knockbackControl; // makes less knockback
       
    // used for perk option stuff
    public bool isPerkOption;
    public PerkManager perks;
    [SerializeField] int perkIndex;

    // assign all variables when enemy is created
    void Awake() {
        appliedDeathForce = false;
        maxHealth = health;
        dead = false;
        spriteRenderer = enemySprite.GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
        
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
            GetComponent<EnemyMovement>().enabled = false; // disable enemy AI because it's dead
            
            // if you didn't already, apply death force to enemy
            if (!appliedDeathForce) {
                Vector3 deathForce = 
                new Vector3(UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10));

                rb.AddForce(deathForce, ForceMode.Impulse);
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
                perks.PerkSelected(perkIndex);

            }
            
            hitCooldown = 0;

            // add knockback force to enemy 
            float randomX = UnityEngine.Random.Range(-knockback * 0.5f, knockback * 0.5f);
            float randomY = UnityEngine.Random.Range(-knockback * 0.5f, knockback * 0.5f);

            // this line used to be >200 chars lmao
            rb.AddForce(new Vector3(randomX, 1, randomY) * knockback * (50 / kbResistance), ForceMode.Force);
            enemyAI.enabled = false;
            
        }

        
        health -= damage;
        
    }

    // set enemy's health
    public void setHealth(float newHealth, String mode) {
        if (mode.Contains("mult")) {
            health *= newHealth;

        } else if (mode.Equals("add")) {
            health += newHealth;

        } else {
            health = newHealth;
        
        }
    }

    // set lieOnFloorTime
    public void setLieOnFloorTime(float newTime) {
        lieOnFloorMaxTime = newTime;

    }

    // get enemy's health 
    public float getHealth() {
        return health;

    }

    // basic setters and getters for index
    public void setIndex(int newIndex) {
        perkIndex = newIndex;

    }

    public int getIndex() {
        return perkIndex;

    }

}
