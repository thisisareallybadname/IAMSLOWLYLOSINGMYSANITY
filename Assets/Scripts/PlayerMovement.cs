using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WhatISThiSUPPSOEDTOBE : MonoBehaviour 
{
    // movement 
    private float xMovement;
    private float zMovement;

    private Vector3 movement;
    private Vector3 yMovement;
    private Vector3 dashMovement;

    private float movementSpeed;
    public float walkspeed;
    public float jumpHeight;

    private bool isRunning;
    private float gravity = -9.8f * 2;

    CharacterController controller;
    GameObject ViewModel;

    Vector3 forward;
    Vector3 right;

    public GameObject groundChecker;
    private bool touchingGround;

    // camera fields
    private float mouseX;
    private float mouseY;

    public Camera playerCam;

    private Vector3 camVector;
    private Vector3 offset = new Vector3(0, 0.5f, 0);

    private float rotationX;
    private float rotationY;

    public float lookspeed;

    private Vector3 cameraBob;
    private float bobX;
    private float bobY;

    private float tick = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        yMovement = Vector3.zero;
        isRunning = false;

    }

    // Update is called once per frame
    void Update()
    {

        movementSpeed = walkspeed;

        touchingGround = Physics.CheckSphere(groundChecker.transform.position, 0.2f);
        controller = GetComponent<CharacterController>();

        xMovement = Input.GetAxis("Horizontal");
        zMovement = Input.GetAxis("Vertical");

        movement = (transform.forward * zMovement) + (transform.right * xMovement);

        Debug.Log(touchingGround);

        updateCamera();

        if (touchingGround && Input.GetAxis("Jump") > 0.25f)
        {
            Debug.Log("JUMP");
            yMovement.y = jumpHeight;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            movement = Vector3.Lerp(movement, movement * movementSpeed, 0.5f);
        }

        yMovement.y += gravity * Time.deltaTime;
        controller.Move(movement * movementSpeed * Time.deltaTime);
        controller.Move(yMovement * Time.deltaTime);

    }

    // this is for camerabob stuff
    Vector3 updateCameraBob() {
        bobX = Mathf.Sin(tick * 0.025f) * 0.05f * movement.magnitude;
        bobY = Mathf.Abs(Mathf.Cos(tick * 0.025f)) * 0.1f * movement.magnitude;
        tick += 1f;

        return new Vector3(bobX, bobY + 0.5f, 0);
    }

    // update camera
    void updateCamera()
    {

        camVector = playerCam.transform.position;
        cameraBob = updateCameraBob();
        offset = touchingGround ? new Vector3(0, 0.5f, 0) + cameraBob : new Vector3(0, 0.5f, 0);

        mouseX = Input.GetAxis("Mouse X") * lookspeed * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * lookspeed * Time.deltaTime;

        // camera movement or smth
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);
        rotationY += mouseX;

        // set cam position and stuff
        playerCam.transform.localPosition = offset;

        playerCam.transform.rotation = Quaternion.Euler(rotationX, rotationY, 0f);
        transform.localRotation = Quaternion.Euler(0f, rotationY, 0f);

    }
    
}
