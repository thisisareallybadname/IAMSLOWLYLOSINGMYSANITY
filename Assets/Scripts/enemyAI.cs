using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class enemyAI : MonoBehaviour {

    private GameObject player;
    public int walkspeed;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
    }

    // Update is called once per frame
    void Update() {
        transform.LookAt(player.transform);
        transform.position += transform.forward * walkspeed * Time.deltaTime;
    }

    private void FixedUpdate() {

    }
}
