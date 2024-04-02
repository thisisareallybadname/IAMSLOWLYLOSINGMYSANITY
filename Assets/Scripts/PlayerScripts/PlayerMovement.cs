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
    private float dashDuration;
    private bool isDashing;

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
    private float dashCooldown = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        yMovement = Vector3.zero;
        gravity = -9.8f * 2;

    }

    // Update is called once per frame
    void Update() {

        movementSpeed = walkspeed;

        touchingGround = Physics.CheckSphere(groundChecker.transform.position, 0.2f);
        movement = (transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal"));

        if (touchingGround && Input.GetAxis("Jump") > 0.25f) {
            yMovement.y = jumpHeight;
            
        } else if (touchingGround) {
            yMovement.y = 0;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            StartCoroutine(Dash(controller, 5, movement));
        }

        yMovement.y += gravity * Time.deltaTime;
        controller.Move(movement * movementSpeed * Time.deltaTime);
        controller.Move(yMovement * Time.deltaTime);

    }

    private void FixedUpdate() {


        if (dashDebounce && dashCooldown < 3) {
            dashCooldown += Time.deltaTime;

        }

        if (dashCooldown > 1) {
            dashDebounce = false;
            dashCooldown = 0;
        }

        if (isDashing) {
            dashDuration += Time.deltaTime;

        } else {
            dashDuration = 0;
        }
        tick += Time.deltaTime;
    }

    IEnumerator Dash(CharacterController characterController, float dashSpeed, Vector3 movement)
    {
        if (!dashDebounce)
        {
            dashDebounce = true;
            isDashing = true;
            while (dashDuration < 0.25f) {
                characterController.Move(movement * 5 * dashSpeed * Time.deltaTime);
                yield return new WaitForSeconds(0.025f * Time.deltaTime);
            }

            isDashing = false;
            
        }
    }

    public float getWalkspeed()
    {
        return walkspeed;
    }

    public float getMovespeed() {
        return movement.magnitude;
    }

}
