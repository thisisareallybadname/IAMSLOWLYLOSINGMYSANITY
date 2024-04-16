using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovementRB : MonoBehaviour
{
    // public fields
    public Rigidbody rb; // character controller
    public GameObject groundChecker; // checks if there's something under player
    public Camera playerCam; // stores player's camera (Usually Main Camera)
    public float lookspeed; // aim sensitivity
    public float walkspeed; // name is pretty self explanatory
    public float jumpHeight; // same thing as above

    // movement 
    private Vector3 movement;
    private Vector3 dashMovement;
    private float dashDuration;
    private bool dashing;
    private bool jumping;

    private float movementX;
    private float movementY;
    private float movementZ;

    private float movementSpeed;
    private float gravity;

    private bool touchingGround;

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
        gravity = -9.8f * 2;

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        movementSpeed = walkspeed;

        touchingGround = Physics.CheckSphere(groundChecker.transform.position, 0.2f);
        //movement = (transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal") + new Vector3(0, Input.GetAxisRaw("Jump"), 0));

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(Dash(5));
        }

        if (touchingGround && Input.GetAxis("Jump") > 0.25f)
        {
            movementY = jumpHeight;

        }
        else if (touchingGround)
        {
            if (movementY < 0) {
                movementY -= 9.8f;
            }
        }

        movementX = Input.GetAxis("Vertical");
        movementZ = Input.GetAxis("Horizontal");

        movement = (transform.forward * movementX) + (transform.right * movementZ);

        if (rb.velocity.magnitude > walkspeed) {
            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        }

        rb.AddForce(movement.normalized * walkspeed * 50, ForceMode.Force);
        tick += Time.deltaTime;
    }

    

    private void Update()
    {

        if (dashDebounce && dashCooldown < 3)
        {
            dashCooldown += Time.deltaTime;

        }

        if (dashCooldown > 1)
        {
            dashDebounce = false;
            dashCooldown = 0;
        }

        if (dashing)
        {
            dashDuration += Time.deltaTime;

        }
        else
        {
            dashDuration = 0;
        }

    }

    IEnumerator Dash(float dashSpeed)
    {
        if (!dashDebounce)
        {
            dashDebounce = true;
            dashing = true;
            while (dashDuration < 0.25f)
            {
                rb.AddForce(movement * 15 * dashSpeed, ForceMode.Impulse);
                yield return new WaitForSeconds(0.025f * Time.deltaTime);
            }

            dashing = false;

        }
    }

    public float getWalkspeed()
    {
        return walkspeed;
    }

    public float getMovespeed()
    {
        return movement.magnitude;
    }
}
