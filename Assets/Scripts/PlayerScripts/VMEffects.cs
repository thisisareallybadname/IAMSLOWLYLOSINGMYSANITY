using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class VMEffects : MonoBehaviour
{
    private float tick;
    [SerializeField] Vector3 bobVector;

    private float bobX;
    private float bobY;

    [SerializeField] GameObject SideofVM;
    [SerializeField] GameObject player;

    PlayerMovement playerScript;

    private float walkspeed;
    private float playerMovespeed;
    private float movespeed;

    [SerializeField] float bobSpeed;
    private float forceSpeed = 1;

    private Vector3 currentPosition;
    private Quaternion currentRotation;

    [SerializeField] Quaternion angleOffset;
    [SerializeField] Vector3 positionOffset;

    private Vector3 VMSway;

    // Start is called before the first frame update
    void Start() {
        playerScript = player.GetComponent<PlayerMovement>();
        walkspeed = playerScript.getWalkspeed();
        currentPosition = positionOffset;
        currentRotation = angleOffset;

    }

    // Update is called once per frame
    void Update() {

        playerMovespeed = playerScript.getMovespeed(); // get player movement magnitude
        movespeed = playerMovespeed > 0.5 ? 1 : 0; // lets bobX and bobY produce values if player moves

        bobX = Mathf.Cos(tick * bobSpeed) * movespeed * 0.125f;
        bobY = Mathf.Abs(Mathf.Sin(tick * bobSpeed)) * movespeed * 0.25f;
        
        if (movespeed == 0) {
            bobX = 0;
            bobY = Mathf.Abs(Mathf.Sin(tick) / 5);
        
        }

        // basically add mouse delta to view model(s), and delay their mvoement and stuff
        VMSway = new Vector3(Input.GetAxisRaw("Mouse X"), -Input.GetAxisRaw("Mouse Y")) / 15f;

        bobVector = new Vector3(bobX, bobY);

        StartCoroutine(updateRecoilForces());
        
        // set positions
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

    // used for recoil effect
    IEnumerator updateRecoilForces() {
        currentPosition = Vector3.Lerp(currentPosition, positionOffset + bobVector + VMSway, 7.5f * Time.deltaTime);
        currentRotation = Quaternion.Slerp(currentRotation, angleOffset, 5 * Time.deltaTime);

        yield return null;

    }
        
    public Vector3 getBobVector() {
        return bobVector;  
    }
}
