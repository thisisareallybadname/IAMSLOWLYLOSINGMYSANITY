using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PlayerKnockback : MonoBehaviour {
    private CharacterController controller;
    private float iFrames;
    private float kbValue;
    
    private bool knockbackDebounce;
    private bool takeKnockback = false;
    
    private float knockback;
    public float knockbackValue;

    private float knockbackDuration;

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

        if (takeKnockback) {
            knockbackDuration += Time.deltaTime;
            knockback *= 0.75f;
        }

        if (knockbackDuration <= 1) {
            knockbackDuration += Time.deltaTime;
        } else {
            knockback = 0;
            knockbackDuration = 0;
        }
    }

    private void FixedUpdate() {
        controller.Move(new Vector3(0, 0, knockback) * Time.deltaTime);

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
            takeKnockback = false;
            knockback = knockbackValue;
            yield return null;

        }
    }


}
