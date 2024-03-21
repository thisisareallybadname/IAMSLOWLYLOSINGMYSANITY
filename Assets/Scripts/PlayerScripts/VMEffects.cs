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

    PlayerMovement playerScript;

    private float walkspeed;
    private float playerMovespeed;
    private float movespeed;

    public float bobSpeed;

    Vector3 force;
    

    // Start is called before the first frame update
    void Start() {
        playerScript = player.GetComponent<PlayerMovement>();
        walkspeed = playerScript.getWalkspeed();
    }

    // Update is called once per frame
    void FixedUpdate() {
        movespeed = playerMovespeed > 0.5 ? 1 : 0;

        bobX = Mathf.Cos(tick * bobSpeed) * movespeed * 7.5f;
        bobY = -Mathf.Abs(Mathf.Sin(tick * bobSpeed)) * movespeed * 7.5f;
        
        if (movespeed == 0) {
            bobX = 0;
            bobY = -Mathf.Abs(Mathf.Sin(tick) / 5) * 50;
        
        }

        bobVector = new Vector3(bobX * 2f, bobY * 1.25f, 0) * 0.5f * Time.deltaTime;

        this.applyForce(bobVector);
        viewModel.transform.localPosition = Vector3.Lerp(viewModel.transform.localPosition, force, 3 * Time.deltaTime);
        tick += Time.deltaTime;
        


    }

    public void setMovespeed(float speed) {
        playerMovespeed = speed;
    }

    public void applyForce(Vector3 newForce) {
        this.force = newForce;
    }
}
