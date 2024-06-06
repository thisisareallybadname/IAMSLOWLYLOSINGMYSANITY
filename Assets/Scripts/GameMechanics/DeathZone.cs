using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour {
    [SerializeField] TimeManager timeManager;
    [SerializeField] PlayerMovement playerMovement;

    // Update is called once per frame
    private void OnTriggerStay(Collider collision) {
        GameObject thingTouchingRigidBody = collision.gameObject;

        if (thingTouchingRigidBody.tag.Equals("Enemy")) {
            thingTouchingRigidBody.GetComponent<EnemyHealth>().takeDamage(10000000, 250);

        } else if (thingTouchingRigidBody.tag.Equals("Player")) {
            if (!timeManager.GameInIntermissionPhase()) {
                thingTouchingRigidBody.GetComponent<PlayerDamage>().takeDamage(10000000000000);

            }

            playerMovement.ResetPlayerPosition();
        }
    }
}
