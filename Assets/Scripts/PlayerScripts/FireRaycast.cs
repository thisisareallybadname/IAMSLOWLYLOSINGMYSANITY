using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRaycast : MonoBehaviour
{
    GameObject shootPos;


    // Start is called before the first frame update
    void Start() {
        shootPos = GameObject.Find("ShootPos");

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;
        if (Input.GetMouseButton(0)) {
            Debug.DrawRay(shootPos.transform.position, shootPos.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            if (Physics.Raycast(shootPos.transform.position, shootPos.transform.TransformDirection(Vector3.forward),out hit, 100)) {
                Debug.DrawRay(shootPos.transform.position, shootPos.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            }
        }
    }
}
