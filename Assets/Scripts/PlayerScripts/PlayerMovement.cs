using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class PlayerMovement : MonoBehaviour
{
    // public fields
    [SerializeField] CharacterController controller; // character controller
    [SerializeField] GameObject groundChecker; // checks if there's something under player
    [SerializeField] Camera playerCam; // stores player's camera (Usually Main Camera)
    [SerializeField] float lookspeed; // aim sensitivity
    [SerializeField] float walkspeed; // name is pretty self explanatory
    [SerializeField] float jumpHeight; // same thing as above

    // movement 
    private Vector3 movement;
    private Vector3 yMovement;
    private Vector3 dashMovement;

    private float movementSpeed;
    private float gravity;

    private bool touchingGround;

    [SerializeField] float speedWhileDashing;

    // camera fields
    private float mouseX;
    private float mouseY;

    private Vector3 camVector;
    private Vector3 offset;

    private float rotationX;
    private float rotationY;

    // fields for ViewModel effects
    private Vector3 cameraBob;

    // dash vars
    private bool dashDebounce = false;
    [SerializeField] float staminaValue;
    [SerializeField] PlayerCamera cam;
    private bool canDash;
    private bool dashing;
    [SerializeField] float staminaLimit = 5;

    private Collider[] dashAOE;
    private RaycastHit hit;

    // Start is called before the first frame update
    void Start() {
        staminaValue = staminaLimit;

        yMovement = Vector3.zero;
        gravity = -9.8f * 2;

    }

    // Update is called once per frame
    void Update() {
        canDash = staminaValue > 0 && !dashing;

        if (staminaValue < staminaLimit && !dashing) {
            staminaValue += Time.deltaTime * 1.5f;

        }

        movementSpeed = walkspeed;

        

        touchingGround = Physics.Raycast(transform.position - new Vector3(0, 1.01f, 0), -transform.up, out hit, 0.1f);
        Debug.DrawRay(transform.position - new Vector3(0, 1.01f, 0), -transform.up);
        
        movement = (transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal"));

        if (touchingGround && Input.GetAxis("Jump") > 0.25f) {
            yMovement.y = jumpHeight;

        }
        else if (touchingGround) {
            yMovement.y = 0;
        }

        if (Input.GetKey(KeyCode.LeftShift) && canDash) {
            StartCoroutine(Dash(speedWhileDashing));
        
        } else {
            dashing = false;

        }

        yMovement.y += gravity * Time.deltaTime;
        controller.Move(movement * movementSpeed * Time.deltaTime);
        controller.Move(yMovement * Time.deltaTime);

    }

    IEnumerator Dash(float dashSpeed) {
        dashDebounce = true;
        dashing = true;

        staminaValue -= Time.deltaTime * 5f;

        controller.Move(movement * dashSpeed * Time.deltaTime);

        dashAOE = Physics.OverlapSphere(transform.position, 3f);
        foreach (Collider c in dashAOE) {
        
            if (c.tag.Equals("Enemy")) {
            c.gameObject.GetComponent<EnemyHealth>().takeDamage(0, 5f);
        
            }
        }

        yield return null;
    }

    public float getWalkspeed() {
        return walkspeed;
    }

    public float getMovespeed() {
        return movement.magnitude;
    }

    public void ResetPlayerPosition() {
        controller.enabled = false;
        transform.position = new Vector3(0, 10, 0);
        controller.enabled = true;
    }

    public void setMovementStats(float newWalkspeed, float newStaminaLimit, string mode) {
        if (mode.Equals("add")) {
            walkspeed += newWalkspeed;
            staminaLimit += newStaminaLimit;

        } else if (mode.Equals("multi")) {
            walkspeed *= newWalkspeed;
            staminaLimit *= newStaminaLimit;

        } else {
            walkspeed = newWalkspeed;
            staminaLimit = newStaminaLimit;

        }
    }

    public float getStaminaValue() {
        return staminaValue;

    }

    public float GetStaminaLimit() {
        return staminaLimit;

    }


    public Vector3 getMovementVector() {
        return movement;

    }

}