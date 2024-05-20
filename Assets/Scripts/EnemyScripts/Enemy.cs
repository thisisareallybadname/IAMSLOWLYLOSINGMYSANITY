using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour {
    public float health;
    public float speed;
    public float damage;

    public Material enemySprite;
    public GameObject enemyPNG;
    
    private enemyAI enemyMovement;
    private EnemyHealth enemyHealth;

    private float variantIndex;

    public bool active;

    void Start() {
        
        enemyHealth = GetComponent<EnemyHealth>();
        enemyMovement = GetComponent<enemyAI>();

        speed = enemyMovement.walkspeed;
        health = enemyHealth.health;

        enemyMovement.MakeHostile(active);
        enemyMovement.setMovementSpeed(speed);

    }

    public float getHealth() {
        return health;
    }

    public float getSpeed() { 
        return speed; 
    }

    public float getDamage() {
        return damage;

    }
    
    public float getIndex() {
        return variantIndex;

    }

    public void setEnemySprite(Material newMaterial) {
        enemyPNG.GetComponent<EnemyHealth>().changeSprite(newMaterial);

    }

    public void setHealth(float health) {
        enemyHealth.health = health;

    }
    public void setDamage(float newDamage) {
        damage = newDamage;

    }

    public void setSpeed(float newSpeed) {
        enemyMovement.walkspeed = (int)newSpeed;

    }

    public void setIndex(float newIndex) {
        variantIndex = newIndex;

    }

    public void setKBResistance(float newKbResistance) {
        enemyHealth.kbResistance = newKbResistance;


    }

}
