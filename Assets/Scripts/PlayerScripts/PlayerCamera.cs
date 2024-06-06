using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class PlayerCamera : MonoBehaviour
{
    private Vector3 camVector;
    public Camera playerCam;

    private float mouseX;
    private float mouseY;

    private float rotationX;
    private float rotationY;

    public float lookspeed;

    private Vector3 offset;
    public PlayerMovement playerMovement;
    private float movespeed; 

    Vector3 force;

    private Vector3 bobVector;
    private float bobX;
    private float bobY;

    private Vector3 newCameraAngle;

    private float tick;

    private VMEffects viewModelEffects;

    // Start is called before the first frame update
    void Start()
    {
        viewModelEffects = GetComponent<VMEffects>();
    }

    private void cameraBob() {
        bobX = Mathf.Cos(tick * 6) * movespeed * 0.125f;
        bobY = -Mathf.Abs(Mathf.Sin(tick * 6)) * movespeed * 0.25f;


        bobVector = new Vector3(bobX, bobY);
    }

    // Update is called once per frame
    void Update() {
        camVector = playerCam.transform.position;

        movespeed = playerMovement.getMovespeed();

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
        force = newForce;
        force.x = Random.Range(-force.x, force.x);
        force.z = Random.Range(-force.z, force.z);

        offset = newOffset;

    }
}
