using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // public fields
    public CharacterController controller; // character controller
    public GameObject groundChecker; // checks if there's something under player
    public Camera playerCam; // stores player's camera (Usually Main Camera)
    public float lookspeed; // aim sensitivity
    public float walkspeed; // name is pretty self explanatory
    public float jumpHeight; // same thing as above

    // movement 
    private Vector3 movement;
    private Vector3 yMovement;
    private Vector3 dashMovement;

    private float movementSpeed;
    private float gravity;

    private bool touchingGround;

    // camera fields
    private float mouseX;
    private float mouseY;

    private Vector3 camVector;
    private Vector3 offset;

    private float rotationX;
    private float rotationY;

    // fields for ViewModel effects
    private Vector3 cameraBob;
    private float tick = 0f;

    // dash vars
    private bool dashDebounce = false;
    public float staminaValue;
    public PlayerCamera cam;
    private bool canDash;
    private bool dashing;
    public float staminaLimit = 5;

    public Collider[] dashAOE;

    // Start is called before the first frame update
    void Start() {
        yMovement = Vector3.zero;
        gravity = -9.8f * 2;

    }

    // Update is called once per frame
    void Update() {
        canDash = staminaValue > 0;

        if (staminaValue < staminaLimit && !dashing) {
            staminaValue += Time.deltaTime * 1.5f;

        }

        movementSpeed = walkspeed;

        touchingGround = Physics.CheckSphere(groundChecker.transform.position, 0.2f);
        movement = (transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal"));

        if (touchingGround && Input.GetAxis("Jump") > 0.25f) {
            yMovement.y = jumpHeight;

        }
        else if (touchingGround) {
            yMovement.y = 0;
        }

        if (Input.GetKey(KeyCode.LeftShift) && canDash) {
            StartCoroutine(Dash(7.5f));
        
        } else {
            dashing = false;

        }

        yMovement.y += gravity * Time.deltaTime;
        controller.Move(movement * movementSpeed * Time.deltaTime);
        controller.Move(yMovement * Time.deltaTime);

    }

    private void FixedUpdate()
    {

        tick += Time.deltaTime;
    }

    IEnumerator Dash(float dashSpeed) {
        dashDebounce = true;
        dashing = true;

        staminaValue -= Time.deltaTime * 2.5f;

        controller.Move(movement * dashSpeed * Time.deltaTime);

        dashAOE = Physics.OverlapSphere(transform.position, 3f);
        foreach (Collider c in dashAOE) {
        
            if (c.tag.Equals("Enemy")) {
            c.gameObject.GetComponent<EnemyHealth>().takeDamage(0, 5f);
        
            }
        }

        yield return null;
    }

    public float getWalkspeed()
    {
        return walkspeed;
    }

    public float getMovespeed()
    {
        return movement.magnitude;
    }

    public Vector3 getMovementVector() {
        return movement;

    }

}