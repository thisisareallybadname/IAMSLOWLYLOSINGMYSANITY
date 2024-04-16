using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    public float health = 5;
    private GameObject collidingGameObject;
    private float hitDebounce;
    private bool canTakeDamage;
    public float hitCooldown;

    public UIManager manager;

    // Start is called before the first frame update
    void Start() {

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
            gameObject.SendMessage("GameOver", true, SendMessageOptions.DontRequireReceiver);

        }


    }

    private void OnTriggerEnter(Collider collision) {
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
