using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAttack : MonoBehaviour
{
    private GameObject weapon;
    private GameObject VM;
    private GameObject shootPos;
    private RaycastHit hit;
    private Quaternion playerDirection;
    public  Camera playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        weapon = GameObject.Find("pistol");
        VM = GameObject.Find("ViewModel");
        shootPos = GameObject.Find("ShootPos");

    }

    private void Update() {

    }

    private void FixedUpdate() {

        if (Input.GetMouseButton(0)) {
            Instantiate(GameObject.Find("cool cube"), playerCamera.transform.position + playerCamera.transform.forward, playerCamera.transform.rotation);
        }
    }

    
}
