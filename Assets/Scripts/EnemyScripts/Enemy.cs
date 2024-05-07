using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour {
    private float health;
    private float speed;
    public float damage;

    private enemyAI enemyMovement;
    private EnemyHealth enemyHealth;

    void Start() {
        enemyHealth = GetComponent<EnemyHealth>();
        enemyMovement = GetComponent<enemyAI>();

        speed = enemyMovement.walkspeed;
        health = enemyHealth.health;

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
}
