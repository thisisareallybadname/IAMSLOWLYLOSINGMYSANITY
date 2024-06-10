using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour {
    [SerializeField] TimeManager timeManager;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] WaveManager waveManager;

    // Update is called once per frame
    private void OnTriggerStay(Collider collision) {
        GameObject thingTouchingRigidBody = collision.gameObject;

        // delete enemy from reality
        if (thingTouchingRigidBody.tag.Equals("Enemy"))
        {
            thingTouchingRigidBody.GetComponent<EnemyHealth>().takeDamage(10000000, 250);

            // delete player from reality
        }
        else if (thingTouchingRigidBody.tag.Equals("Player")) {
            if (waveManager.isRunning()) { 
                thingTouchingRigidBody.GetComponent<PlayerDamage>().setHealth(0);
            }
            // tp player back to spawnpoint so they wont get softlocked at death zone
            playerMovement.SetPosition(new Vector3(0, 10, 0));
        }
    }
}
