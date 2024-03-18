using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class enemyAI : MonoBehaviour {

    private GameObject player;
    public int walkspeed;
    private Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        transform.LookAt(player.transform);
        rigidbody.Move(transform.forward * walkspeed * Time.deltaTime);
    }

    private void FixedUpdate() {

    }
}
