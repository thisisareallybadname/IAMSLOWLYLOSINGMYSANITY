using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Entity {
    private Vector3 movement;
    private float health;
    private float speed;

    public Entity(Vector3 m, float h, float s) {
        movement = m;
        health = h;
        speed = s;

    }

    public Vector3 getMovement() {
        return movement;

    }

    public float getHealth() {
        return health;
    }

    public float getSpeed() { 
        return speed; 
    }
}
