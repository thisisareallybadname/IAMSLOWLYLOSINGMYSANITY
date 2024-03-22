using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;

public class enemyAI : MonoBehaviour {

    public GameObject player;
    public int walkspeed;
    private Rigidbody rigidbody;
    Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {

        transform.LookAt(player.transform);
        transform.position += transform.forward * walkspeed * Time.deltaTime;
    }

    private void FixedUpdate() {

    }
}
