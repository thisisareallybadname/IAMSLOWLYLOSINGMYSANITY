using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAI : MonoBehaviour {

    public GameObject player;
    public int walkspeed;
    public Rigidbody rb;

    private bool chasePlayer;

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void FixedUpdate() {
        transform.LookAt(player.transform.position);

        if (chasePlayer) {
            rb.AddForce(transform.forward * walkspeed * 200 * Time.deltaTime, ForceMode.Force);
            rb.drag = 2.5f;

            rb.velocity += walkspeed * (rb.transform.forward.normalized / 7.5f);
        
        }
    }

    public void setMovementSpeed(float newWalkspeed) {

        walkspeed = (int)newWalkspeed;
    }

    public void MakeHostile(bool chase) {
        chasePlayer = chase;
    }

    

}
