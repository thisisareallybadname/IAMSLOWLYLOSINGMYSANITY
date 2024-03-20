using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private GameObject weapon;
    private GameObject VM;
    private GameObject shootPos;
    private RaycastHit hit;
    private Quaternion playerDirection;

    private PlayerCamera playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        weapon = GameObject.Find("pistol");
        VM = GameObject.Find("ViewModel");
        shootPos = GameObject.Find("ShootPos");
        playerCamera= GetComponent<PlayerCamera>();

    }

    private void Update() {
    }
    private void FixedUpdate() {

        if (Input.GetMouseButtonDown(0)) {
            Debug.Log("*click*");
            if (Physics.Raycast(shootPos.transform.position, shootPos.transform.forward, 100))
            {
                Debug.Log("pew pew");
            }
        }
    }

    
}
