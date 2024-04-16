using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class VMEffects : MonoBehaviour
{
    private float tick;
    private Vector3 bobVector;
    private Vector3 target;

    private float bobX;
    private float bobY;

    public GameObject arm;
    public GameObject player;

    private FireWeapon checkForFiring;

    PlayerMovement playerScript;

    private float walkspeed;
    private float playerMovespeed;
    private float movespeed;

    public float bobSpeed;
    private float forceSpeed = 1;

    private Vector3 currentPosition;
    private Vector3 currentRotation;

    public Vector3 angleOffset;
    public Vector3 positionOffset;

    private bool isFiring = false;

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
        
        if (currentPosition.magnitude <= 0.5f) {
            bobX = 0;
            bobY = -Mathf.Abs(Mathf.Sin(tick) / 5);
        
        }

        if (forceSpeed == 1) {
            bobVector = new Vector3(bobX, bobY, 0) * 0.75f;
 
        } else {
            bobVector = Vector3.zero;
        
        }
        StartCoroutine(updateRecoilForces());
        arm.transform.localPosition = currentPosition;
        arm.transform.localRotation = Quaternion.Euler(currentRotation);

        tick += Time.deltaTime;


    }

    public void setMovespeed(float speed) {
        playerMovespeed = speed;
    }

    public void applyForce(Vector3 newForce, float speed, Vector3 newRotation) {
        currentPosition = newForce + positionOffset;
        currentRotation = newRotation + angleOffset;
        forceSpeed = speed;

    }

    IEnumerator updateRecoilForces() {
        currentPosition = Vector3.Lerp(currentPosition, positionOffset + bobVector, 5 * Time.deltaTime * forceSpeed);
        currentRotation = Vector3.Slerp(currentRotation, angleOffset, 5 * Time.deltaTime);

        forceSpeed = Mathf.Lerp(forceSpeed, 1, 5 * Time.deltaTime);
        yield return null;

    }
}
