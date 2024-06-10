using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

// controls the player's camera
public class PlayerCamera : MonoBehaviour {

    // player's camera
    [SerializeField] Camera playerCam;

    // mouse delta coords
    private float mouseX;
    private float mouseY;

    // rotation values
    private float rotationX;
    private float rotationY;

    // camera sensitivity
    [SerializeField] float lookspeed;

    // camera force stuff
    private Vector3 force;
    private Quaternion forceAngle;

    // used to get walkspeed
    [SerializeField] PlayerMovement playerMovement;
    private float movespeed;

    private Quaternion cameraGoal;
    
    private float tick; // used for bobVector

    private void Start() {
        Application.targetFrameRate = 30;
    }

    // Update is called once per frame
    void Update() {
        movespeed = playerMovement.getMovespeed();

        // get mouse deltas
        mouseX = Input.GetAxis("Mouse X") * lookspeed * Time.smoothDeltaTime;
        mouseY = Input.GetAxis("Mouse Y") * lookspeed * Time.smoothDeltaTime;

        // rotationX is on the x-axis, so adding mouse-y delta will make it move up/down
        // same thing with rotationY  & adding mouse-x delta, but its on the x-axis, so it moves left-right
        rotationX -= mouseY + force.y;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);
        rotationY += mouseX + force.x;
        
        // make force and offset diminish until they reach vector3.zero
        force = Vector3.Lerp(force, Vector3.zero, Time.smoothDeltaTime * 5);
        forceAngle = Quaternion.Lerp(forceAngle, Quaternion.identity, Time.smoothDeltaTime * 5);

        // set positions

        cameraGoal = Quaternion.Euler(rotationX, rotationY, force.z) * forceAngle;
        playerCam.transform.localPosition =  force + new Vector3(0, 0.5f, 0);
        playerCam.transform.rotation = 
        Quaternion.Slerp(playerCam.transform.rotation, cameraGoal, Time.deltaTime * 25);
        transform.localRotation = Quaternion.Euler(0f, rotationY, 0);

        tick += Time.deltaTime;
    }

    // set force and offset and stuff
    public void applyCameraForce(Vector3 newForce, Quaternion newOffset) {
        force += newForce; 
        force.x = Random.Range(-force.x, force.x);
        force.z = Random.Range(-force.z, force.z);

        forceAngle *= newOffset;

    }
}
