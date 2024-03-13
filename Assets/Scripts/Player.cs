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
    private Boolean isDashing;

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

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        yMovement = Vector3.zero;
        gravity = -9.8f * 2;

    }

    // Update is called once per frame
    void Update()
    {

        movementSpeed = walkspeed;

        touchingGround = Physics.CheckSphere(groundChecker.transform.position, 0.2f);
        controller = GetComponent<CharacterController>();

        movement = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
        updateCamera();

        if (touchingGround && Input.GetAxis("Jump") > 0.25f)
        {
            Debug.Log("JUMP");
            yMovement.y = jumpHeight;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            StartCoroutine(Dash(controller, 5, movement));
        }

        yMovement.y += gravity * Time.deltaTime;
        controller.Move(movement * movementSpeed * Time.deltaTime);
        controller.Move(yMovement * Time.deltaTime);

    }

    // this is for camerabob stuff

    float bobX(float amp, float speed) { 
        return Mathf.Sin(tick * speed / 2) * amp * 3f;
    }

    float bobY(float amp, float speed) {
        return Mathf.Abs(Mathf.Cos(tick * speed / 2)) * amp;
    }
    Vector3 bobEffect(float amp, float speed) {
        return new Vector3(bobX(amp * 3f, speed * 2), bobY(amp, speed), 0);
    }

    void updateVMEffects() { 
        VM.transform.localPosition = Vector3.Lerp(VM.transform.localPosition, bobEffect(movement.magnitude * 0.01f, 0.05f) - new Vector3(0, 0.5f, 0), 3 * Time.deltaTime);
        VM.transform.localRotation = Quaternion.Euler(0, 0, -bobY(movement.magnitude * 2f, 0.05f) + 1.15f);
        tick += 1f; // TO BE REPLACED
    }

    // update camera
    void updateCamera() {

        camVector = playerCam.transform.position;
        cameraBob = Vector3.Lerp(VM.transform.localPosition, bobEffect(movement.magnitude * 0.01f, 0.01f), 3 * Time.deltaTime);
        cameraBob.x = -cameraBob.x; // lemme cook

        mouseX = Input.GetAxisRaw("Mouse X") * lookspeed * Time.deltaTime;
        mouseY = Input.GetAxisRaw("Mouse Y") * lookspeed * Time.deltaTime;

        // camera movement or smth
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);
        rotationY += mouseX;

        updateVMEffects();

        playerCam.transform.localPosition = new Vector3(0, 1f, 0) + cameraBob;
        playerCam.transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        transform.localRotation = Quaternion.Euler(0f, rotationY, -bobY(movement.magnitude * 2f, 0.01f) + 1.15f);

    }

    IEnumerator Dash(CharacterController characterController, float dashSpeed, Vector3 movement)
    {
        for (float i = 0f; i < 10; i += 1f) {
            if (isDashing == false) {
                isDashing = true;
            }
            characterController.Move(movement * 25 * dashSpeed * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }
        isDashing = false;

    }

    private void FixedUpdate()
    {
        Collider[] minorTouchers = Physics.OverlapBox(transform.position, transform.localScale / 2, transform.localRotation, LayerMask.NameToLayer("Hitboxes"));
        Debug.Log(minorTouchers.Length);
    }

    Boolean Dashing() {
        return isDashing;
    }

}
