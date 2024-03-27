using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    private float health = 5;
    private GameObject collidingGameObject;
    private float hitDebounce;
    private bool canTakeDamage;
    public float hitCooldown;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {

        Debug.Log(health);

        if (!canTakeDamage) {
            hitDebounce += Time.fixedDeltaTime;

            if (hitDebounce >= hitCooldown) {
                canTakeDamage = true;

            }

        } else {
            hitDebounce = 0;

        }

        if (health <= 0) {
            Destroy(this.gameObject);

        }
    }

    private void OnCollisionEnter(Collision collision) {

        collidingGameObject = collision.gameObject;

        if (canTakeDamage)
        {
            if (collidingGameObject.tag.Equals("Enemy"))
            {
                health -= 1;
                canTakeDamage = false;
            }

        }
    }

    public void setHealth(float newHealth) {
        health = newHealth;
    }
}
