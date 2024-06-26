using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// kills entities that touch death zone
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
            while (thingTouchingRigidBody.transform.position != new Vector3(0, 10, 0)) {
                thingTouchingRigidBody.GetComponent<CharacterController>().enabled = false;
                thingTouchingRigidBody.transform.position = new Vector3(0, 10, 0);

            }

            thingTouchingRigidBody.GetComponent<CharacterController>().enabled = true;
        }
    }
}
