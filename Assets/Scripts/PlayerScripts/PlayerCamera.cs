using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private Vector3 camVector;
    public Camera playerCam;
    private float mouseX;
    private float mouseY;
    private float rotationX;
    private float rotationY;

    public float lookspeed;

    private VMEffects viewModelEffects;

    // Start is called before the first frame update
    void Start()
    {
        viewModelEffects = GetComponent<VMEffects>();
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        camVector = playerCam.transform.position;

        mouseX = Input.GetAxisRaw("Mouse X") * lookspeed * Time.deltaTime;
        mouseY = Input.GetAxisRaw("Mouse Y") * lookspeed * Time.deltaTime;

        // camera movement or smth
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);
        rotationY += mouseX;

        playerCam.transform.localPosition = new Vector3(0, 0.5f, 0);
        playerCam.transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        transform.localRotation = Quaternion.Euler(0f, rotationY, 0);
    }

    public Quaternion getPlayerRotation() {
        return playerCam.transform.rotation;

    }

    public Quaternion getLocalPlayerRotation() { 
        return playerCam.transform.localRotation;

    }
}
