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
    public GameObject VM; // ViewModel field, the fake set of arms in first person mode

    // movement 
    private Vector3 movement;
    private Vector3 yMovement;
    private Vector3 dashMovement;
    private float dashDuration;
    private bool isDashing;

    private float movementSpeed;
    private float gravity;

    private bool touchingGround;

    public VMEffects vmEffects;

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
        controller = GetComponent<CharacterController>();
        vmEffects = GetComponent<VMEffects>();

        movement = (transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal"));

        if (touchingGround && Input.GetAxis("Jump") > 0.25f)
        {
            yMovement.y = jumpHeight;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            StartCoroutine(Dash(controller, 5, movement));
        }

        yMovement.y += gravity * Time.deltaTime;
        controller.Move(movement * movementSpeed * Time.deltaTime);
        controller.Move(yMovement * Time.deltaTime);

        vmEffects.setMovement(movement.magnitude);

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
}
