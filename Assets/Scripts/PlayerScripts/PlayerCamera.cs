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

    private float tick;

    private VMEffects viewModelEffects;

    // Start is called before the first frame update
    void Start()
    {
        viewModelEffects = GetComponent<VMEffects>();
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    private void cameraBob() {
        bobX = Mathf.Cos(tick * 6) * movespeed * 0.25f;
        bobY = Mathf.Abs(Mathf.Sin(tick * 6)) * movespeed * 0.5f;


        bobVector = new Vector3(bobX, bobY);
    }

    // Update is called once per frame
    void Update() {
        camVector = playerCam.transform.position;

        movespeed = playerMovement.getMovespeed();

        mouseX = Input.GetAxisRaw("Mouse X") * lookspeed * Time.deltaTime;
        mouseY = Input.GetAxisRaw("Mouse Y") * lookspeed * Time.deltaTime;

        // camera movement or smth
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);
        rotationY += mouseX;

        rotationX -= force.y;
        rotationY -= force.x;

        force = Vector3.Lerp(force, Vector3.zero, Time.deltaTime * 5);
        offset = Vector3.Lerp(offset, new Vector3(0, 0.5f, 0) + bobVector, Time.deltaTime * 5);

        playerCam.transform.localPosition = offset;
        playerCam.transform.rotation = Quaternion.Euler(rotationX, rotationY, force.z);
        transform.localRotation = Quaternion.Euler(0f, rotationY, 0);

        tick += Time.deltaTime;
    }

    public Quaternion getPlayerRotation() {
        return playerCam.transform.rotation;

    }

    public Quaternion getLocalPlayerRotation() { 
        return playerCam.transform.localRotation;

    }

    public void applyCameraForce(Vector3 newForce, Vector3 newOffset) {
        force = newForce;
        force.x = Random.Range(-force.x, force.x);
        force.z = Random.Range(-force.z, force.z);

        offset = newOffset;

    }
}
