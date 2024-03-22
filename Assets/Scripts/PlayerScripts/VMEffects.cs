using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class VMEffects : MonoBehaviour
{
    private float tick;
    private Vector3 origin;
    private Vector3 bobVector;
    private Vector3 target;

    private float bobX;
    private float bobY;

    public GameObject viewModel;
    public GameObject player;
    public GameObject camera;

    private FireWeapon checkForFiring;

    PlayerMovement playerScript;

    private float walkspeed;
    private float playerMovespeed;
    private float movespeed;

    public float bobSpeed;
    private float forceSpeed;

    private bool isFiring = false;
    Vector3 force;
    

    // Start is called before the first frame update
    void Start() {
        playerScript = GetComponent<PlayerMovement>();
        walkspeed = playerScript.getWalkspeed();
        checkForFiring = GetComponent<FireWeapon>();
    }

    // Update is called once per frame
    void FixedUpdate() {

        isFiring = checkForFiring.isFiring();

        movespeed = playerMovespeed > 0.5 ? 1 : 0;

        bobX = Mathf.Cos(tick * bobSpeed) * movespeed * 7.5f;
        bobY = -Mathf.Abs(Mathf.Sin(tick * bobSpeed)) * movespeed * 7.5f;
        
        if (movespeed == 0) {
            bobX = 0;
            bobY = -Mathf.Abs(Mathf.Sin(tick) / 5) * 50;
        
        }

        bobVector = new Vector3(bobX * 2f, bobY * 1.25f, 0) * 0.5f * Time.deltaTime;

        if (isFiring)
        {
            bobVector /= 5;
            forceSpeed = 20;

        } else {
            forceSpeed = 3;
        }

        //this.applyForce(bobVector, 3);
        viewModel.transform.localPosition = Vector3.Lerp(viewModel.transform.localPosition, bobVector + force, forceSpeed * Time.deltaTime);
        tick += Time.deltaTime;


    }

    public void setMovespeed(float speed) {
        playerMovespeed = speed;
    }

    public void applyForce(Vector3 newForce, float speed) {
        this.force = newForce;
        forceSpeed = speed;
    }
}
