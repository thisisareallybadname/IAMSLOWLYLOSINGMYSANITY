using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag.Equals("Enemy") && collision.gameObject.GetComponent<enemyAI>().chasePlayer) {
            collision.gameObject.GetComponent<Enemy>().takeDamage(100);

        } else if (collision.gameObject.tag.Equals("Player")) {
            Debug.Log("HELP");
            collision.gameObject.GetComponent<PlayerHealth>().setHealth(100);

        }
    }
}
