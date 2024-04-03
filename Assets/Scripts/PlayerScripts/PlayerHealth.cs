using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    public float health = 5;
    private GameObject collidingGameObject;
    private float hitDebounce;
    private bool canTakeDamage;
    public float hitCooldown;

    public GameObject player;
    public Camera playerCam;
    public GameObject deathCam;

    public CharacterController controller;
    private Rigidbody rb;
    public UIManager manager;

    // Start is called before the first frame update
    void Start() {
        playerCam.enabled = true;
        deathCam.GetComponent<Camera>().enabled = false;
        controller.enabled = true;
    }

    // Update is called once per frame
    void Update() {

        if (!canTakeDamage) {
            hitDebounce += Time.fixedDeltaTime;

            if (hitDebounce >= hitCooldown) {
                canTakeDamage = true;

            }

        } else {
            hitDebounce = 0;

        }

        if (health <= 0) {
            deathCam.GetComponent<Camera>().enabled = true;
            controller.enabled = false;
            manager.die();
            Destroy(this.gameObject);

        }
    }

    private void OnCollisionEnter(Collision collision) {

        collidingGameObject = collision.gameObject;

        if (canTakeDamage)
        {
            if (collidingGameObject.tag.Equals("Enemy"))
            {
                manager.PlrTakeDamage(1);

                health -= 1;
                canTakeDamage = false;
            }

        }
    }

    public void setHealth(float newHealth) {
        health = newHealth;
    }

    public float getHealth() {
        return health;
    }
}
