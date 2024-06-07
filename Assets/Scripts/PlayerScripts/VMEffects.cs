using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class VMEffects : MonoBehaviour
{
    private float tick;
    [HideInInspector] public Vector3 bobVector;
    private Vector3 target;

    private float bobX;
    private float bobY;

    public GameObject SideofVM;
    public GameObject player;

    private FireWeapon checkForFiring;

    PlayerMovement playerScript;

    private float walkspeed;
    private float playerMovespeed;
    private float movespeed;

    public float bobSpeed;
    private float forceSpeed = 1;

    private Vector3 currentPosition;
    private Quaternion currentRotation;

    public Quaternion angleOffset;
    public Vector3 positionOffset;

    private bool isFiring = false;

    private Vector3 VMSway;

    public Camera playerCam;
    // Start is called before the first frame update
    void Start() {
        playerScript = player.GetComponent<PlayerMovement>();
        walkspeed = playerScript.getWalkspeed();
        currentPosition = positionOffset;
        currentRotation = angleOffset;

    }

    // Update is called once per frame
    void Update() {

        playerMovespeed = playerScript.getMovespeed();
        movespeed = playerMovespeed > 0.5 ? 1 : 0;

        bobX = Mathf.Cos(tick * bobSpeed) * movespeed * 0.25f;
        bobY = -Mathf.Abs(Mathf.Sin(tick * bobSpeed)) * movespeed * 0.25f;
        
        if (movespeed == 0) {
            bobX = 0;
            bobY = -Mathf.Abs(Mathf.Sin(tick) / 5);
        
        }

        VMSway = new Vector3(Input.GetAxisRaw("Mouse X"), -Input.GetAxisRaw("Mouse Y")) / 7.5f;
        
        if (forceSpeed == 1) {
            bobVector = new Vector3(bobX, bobY);
        } else {
            bobVector = Vector3.zero;
        
        }
        StartCoroutine(updateRecoilForces());
        SideofVM.transform.localPosition = currentPosition;
        SideofVM.transform.localRotation = currentRotation;

        tick += Time.deltaTime;

    }

    public void setMovespeed(float speed) {
        playerMovespeed = speed;
    }

    public void applyForce(Vector3 newForce, float speed, Quaternion newRotation) {
        currentPosition = newForce + positionOffset;
        currentRotation = newRotation * angleOffset;
        //forceSpeed = speed;


    }

    IEnumerator updateRecoilForces() {
        currentPosition = Vector3.Lerp(currentPosition, positionOffset + bobVector, 7.5f * Time.deltaTime);
        currentRotation = Quaternion.Slerp(currentRotation, angleOffset, 5 * Time.deltaTime);

        yield return null;

    }
        
    public Vector3 getBobVector() {
        return bobVector;  
    }
}
