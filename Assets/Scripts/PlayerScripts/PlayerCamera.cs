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
    private Vector3 offset;

    // used to get walkspeed
    [SerializeField] PlayerMovement playerMovement;
    private float movespeed; 

    // bob vector stuff
    private Vector3 bobVector;
    private float bobX;
    private float bobY;
    
    private float tick; // used for bobVector

    // generate cameraBob
    private void cameraBob() {
        bobX = Mathf.Cos(tick * 6) * movespeed * 0.125f;
        bobY = -Mathf.Abs(Mathf.Sin(tick * 6)) * movespeed * 0.25f;


        bobVector = new Vector3(bobX, bobY);
    }

    // Update is called once per frame
    void Update() {
        movespeed = playerMovement.getMovespeed();

        // get mouse deltas
        mouseX = Input.GetAxis("Mouse X") * lookspeed * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * lookspeed * Time.deltaTime;

        // rotationX is on the x-axis, so adding mouse-y delta will make it move up/down
        // same thing with rotationY  & adding mouse-x delta, but its on the x-axis, so it moves left-right
        rotationX -= mouseY - force.y;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);
        rotationY += mouseX - force.x;
    `    
        // make force and offset diminish until they reach vector3.zero
        force = Vector3.Lerp(force, Vector3.zero, Time.deltaTime * 5);
        offset = Vector3.Lerp(offset, new Vector3(0, 0.5f, 0) + bobVector, Time.deltaTime * 5);

        // set positions
        playerCam.transform.localPosition = offset + force;
        playerCam.transform.rotation = Quaternion.Euler(rotationX, rotationY, force.z);
        transform.localRotation = Quaternion.Euler(0f, rotationY, 0);

        tick += Time.deltaTime;
    }

    // set force and offset and stuff
    public void applyCameraForce(Vector3 newForce, Vector3 newOffset) {
        force += newForce; 
        force.x = Random.Range(-force.x, force.x);
        force.z = Random.Range(-force.z, force.z);

        offset += newOffset;

    }
}
