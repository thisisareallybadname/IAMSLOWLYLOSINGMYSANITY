using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAI : MonoBehaviour {

    public GameObject player;
    public int walkspeed;
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += transform.forward * walkspeed * Time.deltaTime;
        transform.LookAt(player.transform.position);
        rb.AddForce(transform.forward * walkspeed * Time.deltaTime, ForceMode.Impulse);
        rb.velocity = Vector3.Normalize(rb.velocity) * walkspeed;
    }
    

}
