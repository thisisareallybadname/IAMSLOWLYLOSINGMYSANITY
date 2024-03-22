using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VMEffects : MonoBehaviour
{
    private float tick;
    public GameObject VM;

    private float bobX;
    private float bobY;
    private Vector3 bobVector;

    public float speed;
    private float playerMovement;
    private float movementValue;

    private float playerWalkspeed;
    // Start is called before the first frame update
    void Start()
    {
        playerWalkspeed = GetComponent<PlayerMovement>().walkspeed;
    }

    void Update() {
        movementValue = playerMovement > 0.5 ? playerWalkspeed : 0;

        bobX = Mathf.Cos(tick * speed) * movementValue * 3;
        bobY = -Mathf.Abs(Mathf.Sin(tick * speed)) * movementValue;
        bobVector = new Vector3(bobX, bobY, 0) * Time.deltaTime;

        VM.transform.localPosition = Vector3.Lerp(VM.transform.localPosition, bobVector, Time.deltaTime);
        tick += Time.deltaTime;
    }

    public void setMovement(float movement) {
        playerMovement = movement;

    }
}
