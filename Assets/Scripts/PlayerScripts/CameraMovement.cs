using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    public float cameraSensitivity;

    private float rotationX = 0f;
    private float rotationY = 0f;

    public Camera playerCam;

    // Start is called before the first frame update
    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update() {

        float mouseX = Input.GetAxisRaw("Mouse X") * cameraSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * cameraSensitivity * Time.deltaTime;

        rotationX -= mouseY;

        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        rotationY += mouseX;

        playerCam.transform.localPosition = Vector3.Lerp(playerCam.transform.localPosition, new Vector3(0, 0.7f, -0.2f), Time.deltaTime * 0.25f);
        playerCam.transform.rotation = Quaternion.Euler(rotationX, rotationY, 0f);
        transform.localRotation = Quaternion.Euler(0f, rotationY, 0f);
    }
}
