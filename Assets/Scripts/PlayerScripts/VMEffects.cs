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

    

    // Start is called before the first frame update
    void Start() {
        playerScript = player.GetComponent<PlayerMovement>();
        walkspeed = playerScript.getWalkspeed();
    }

    // Update is called once per frame
    void Update() {
        movespeed = playerMovespeed > 0.25 ? 1 : 0;

        bobX = Mathf.Cos(tick * bobSpeed) * movespeed * 35;
        bobY = -Mathf.Abs(Mathf.Sin(tick * bobSpeed)) * movespeed * 20;
        
        if (movespeed == 0) {
            bobX = 0;
            bobY = -Mathf.Abs(Mathf.Sin(tick)) * 50;
        }

        bobVector = new Vector3(bobX * 2f, bobY * 1.25f, 0) * 0.5f * Time.deltaTime;

        

        viewModel.transform.localPosition = Vector3.Lerp(viewModel.transform.localPosition, bobVector, 3 * Time.deltaTime);

        tick += Time.deltaTime;
        


    }

    public void setMovespeed(float speed) {
        playerMovespeed = speed;
    }
}
