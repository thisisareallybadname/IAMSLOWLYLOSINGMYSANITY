using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAI : MonoBehaviour {

    public GameObject player;
    public int walkspeed;
    public Rigidbody rb;

    public bool chasePlayer;

    // Start is called before the first frame update
    void Start() {


    }

    // Update is called once per frame
    void FixedUpdate() {
        if (chasePlayer) {
            transform.LookAt(player.transform.position);
            rb.AddForce(transform.forward * walkspeed * Time.deltaTime, ForceMode.Force);
            rb.velocity += rb.transform.forward.normalized / 5;
        
        } else {
            rb.Sleep();
            transform.position = new Vector3(0, -100, 0);
        }
    }
    

}
