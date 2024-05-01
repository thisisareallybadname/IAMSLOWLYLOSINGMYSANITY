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

    // movement vector3s
    private Vector3 movement;
    private Vector3 yMovement;
    private Vector3 dashMovement;

    // dash variables
    private float dashDuration;
    private bool isDashing;

    // gravity and stuff
    private float movementSpeed;
    private float gravity;

    // stores true/false if player is touching ground or not
    private bool touchingGround;

    // dash vars
    private bool dashDebounce = false;
    private float dashCooldown = 0f;
    public PlayerCamera cam;

    public Collider[] takeDashDamage;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        yMovement = Vector3.zero;
        gravity = -9.8f * 2;
        movementSpeed = walkspeed;

    }

    // Update is called once per frame
    void Update() {

        // check if player is touching a surface at the bottom
        touchingGround = Physics.CheckSphere(groundChecker.transform.position, 0.2f);

        // assign movement direction
        // Input.GetAxisRaw("Vertical") gives a value between -1 & 1 depending if you press W or S
        // Input.GetAxisRaw("Horizontal") gives a value between -1 & 1 depending if you press A or D
        // vector is defined by transform.forward by W/S movement, and add transform.right * A/D movement

        movement = (transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal"));

        // jump controls
        if (touchingGround && Input.GetAxis("Jump") > 0.25f) {
            yMovement.y = jumpHeight;

        // reset gravity when you touch ground            
        } else if (touchingGround) {
            yMovement.y = 0;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            StartCoroutine(Dash(controller, 5, movement));
        }
        
        // add gravity to player's character controller (move it down)
        yMovement.y += gravity * Time.deltaTime;
        controller.Move(yMovement * Time.deltaTime);

        // move character controller with wasd
        controller.Move(movement * movementSpeed * Time.deltaTime);

        UpdateDash();
    }


    private void UpdateDash() {
        if (dashDebounce && dashCooldown < 3)
        {
            dashCooldown += Time.deltaTime;

        }

        if (dashCooldown > 1)
        {
            dashDebounce = false;
            dashCooldown = 0;
        }

        if (isDashing) {
            dashDuration += Time.deltaTime;

        }
        else {
            dashDuration = 0;

        }

    }

    // when LSHIFT pressed + cooldown gone, dash
    IEnumerator Dash(CharacterController characterController, float dashSpeed, Vector3 movement) {
        if (!dashDebounce)
        {
            dashDebounce = true;
            isDashing = true;

            takeDashDamage = Physics.OverlapBox(transform.position, new Vector3(250f, 250f, 250f), transform.rotation);

            foreach (Collider c in takeDashDamage) {
                if (c.tag.Equals("Enemy")) {
                    c.gameObject.GetComponent<EnemyHealth>().takeDamage(9999999);
                }

            }

            while (dashDuration < 0.25f) {

                // move player forward 
                characterController.Move(movement * 5 * dashSpeed * Time.deltaTime);
                
                // change FOV when dashing
                cam.applyCameraForce(Vector3.zero, new Vector3(0, 0.25f, -0.5f));
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
