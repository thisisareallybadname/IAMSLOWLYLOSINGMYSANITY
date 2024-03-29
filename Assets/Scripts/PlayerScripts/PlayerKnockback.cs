using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockback : MonoBehaviour {
    private CharacterController controller;
    private float iFrames;
    private float kbValue;
    private bool knockbackDebounce;
    private bool takeKnockback = false;

    // Start is called before the first frame update
    void Start() {
        controller = GetComponent<CharacterController>();
    
    }

    // Update is called once per frame
    void Update() {

        StartCoroutine(KnockbackPlayer());
        if (knockbackDebounce) {
            while (kbValue < 0.5f) {
                kbValue += Time.deltaTime;
            }

        } else {
            kbValue = 0f;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag.Equals("Enemy")) {
            takeKnockback = true;

        }

    }

    private void OnCollisionExit(Collision collision) {
        if (collision.gameObject.tag.Equals("Enemy")) {
            takeKnockback = false;

        }

    }

    IEnumerator KnockbackPlayer()
    {
        if (takeKnockback) {
            controller.Move(transform.forward * -100 * Time.deltaTime);
            yield return null;
        }
    }
}
