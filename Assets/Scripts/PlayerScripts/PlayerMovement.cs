using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Json;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

// moves the player around the map
public class PlayerMovement : MonoBehaviour
{
    // public fields
    [SerializeField] CharacterController controller; // character controller
    [SerializeField] GameObject groundChecker; // checks if there's something under player
    [SerializeField] Camera playerCam; // stores player's camera (Usually Main Camera)
    [SerializeField] float lookspeed; // aim sensitivity
    [SerializeField] float walkspeed; // name is pretty self explanatory
    [SerializeField] float jumpHeight; // same thing as above

    [SerializeField] WaveManager waveManager;
    [SerializeField] PlayerDamage damage;
    // movement variables
    private Vector3 movement;
    private Vector3 yMovement;
    private Vector3 dashMovement;

    private float gravity;

    private bool touchingGround;

    [SerializeField] float speedWhileDashing;

    // dash vars
    private bool dashDebounce = false;
    [SerializeField] float staminaValue;
    [SerializeField] PlayerCamera cam;
    private bool canDash;
    private bool dashing;
    [SerializeField] float staminaLimit = 5;

    private Collider[] dashAOE;
    private RaycastHit hit;

    // Start is called before the first frame update
    void Start() {
        staminaValue = staminaLimit;

        yMovement = Vector3.zero;
        gravity = -9.8f * 2;

    }

    // Update is called once per frame
    void Update() {
        canDash = staminaValue > 0 && !dashing;
        // make stamina regenerate
        if (staminaValue < staminaLimit && !dashing) {
            staminaValue += Time.deltaTime * 1.5f;

        }

        // use raycast facing down to check if player is touching ground
        touchingGround = 
        Physics.Raycast(transform.position - new Vector3(0, 1.01f, 0), -transform.up, 0.1f);

        // xMovement, zMovement, yMovement, and the char controller thingy are from this vid
        //https://www.youtube.com/watch?v=Nxg0vQk05os&list=PLtLToKUhgzwnk4U2eQYridNnObc2gqWo-

        Vector3 xMovement = transform.forward * Input.GetAxisRaw("Vertical");
        Vector3 zMovement = transform.right * Input.GetAxisRaw("Horizontal");

        movement = xMovement + zMovement;

        if (touchingGround && Input.GetAxis("Jump") > 0.25f) {
            yMovement.y = jumpHeight;

        }
        else if (touchingGround) {
            yMovement.y = 0;
        }

        // dash when leftshift is held down
        if (Input.GetKey(KeyCode.LeftShift) && canDash && movement.magnitude > 0.5f) {
            dashing = true;
            StartCoroutine(Dash(speedWhileDashing));
            
        } else {
            dashing = false;

        }
     
        if (transform.position.y < -10) {
            SetPosition(new Vector3(0, 10, 0));
            if (waveManager.isRunning()) {
                damage.setHealth(0);

            }
        }

        yMovement.y += gravity * Time.deltaTime;
        controller.Move((movement * walkspeed + yMovement) * Time.deltaTime);
        //controller.Move(yMovement * Time.deltaTime);

    }

    // make the player move faster temporarially
    IEnumerator Dash(float dashSpeed) {
        dashDebounce = true;
        dashing = true;

        staminaValue -= Time.deltaTime * 5f;

        controller.Move(movement * dashSpeed * Time.deltaTime);

        dashAOE = Physics.OverlapSphere(transform.position, 3f);
        foreach (Collider c in dashAOE) {
        
            if (c.tag.Equals("Enemy")) {
            c.gameObject.GetComponent<EnemyHealth>().takeDamage(0, 1.25f);
        
            }
        }

        yield return null;
    }

    public float getWalkspeed() {
        return walkspeed;
    }

    public float getMovespeed() {
        return movement.magnitude;
    }

    public void SetPosition(Vector3 position) {
        StartCoroutine(SetPositionOfPlayer(position));
    }

    IEnumerator SetPositionOfPlayer(Vector3 position) {
        while (transform.position != position) {
            staminaValue = 0;
            controller.enabled = false;
            transform.position = position;

        }
        
        yield return null;
        controller.enabled = true;
    }

    // sets movement stats to whatever's in parameters, depending on mode
    public void setMovementStats(float newWalkspeed, float newStaminaLimit, string mode) {
        if (mode.Equals("add")) {
            walkspeed += newWalkspeed;
            staminaLimit += newStaminaLimit;

        } else if (mode.Equals("multi")) {
            walkspeed *= newWalkspeed;
            staminaLimit *= newStaminaLimit;

        } else {
            walkspeed = newWalkspeed;
            staminaLimit = newStaminaLimit;

        }
        staminaValue = staminaLimit;
    }

    public float getStaminaValue() {
        return staminaValue;

    }

    public float GetStaminaLimit() {
        return staminaLimit;

    }


    public Vector3 getMovementVector() {
        return movement;

    }

}