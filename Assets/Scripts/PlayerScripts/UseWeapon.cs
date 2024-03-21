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
        weapon = GameObject.Find("weapon");
        VM = GameObject.Find("ViewModel");
        shootPos = GameObject.Find("ShootPos");

    }

    private void Update() {

    }

    private void FixedUpdate() {

        if (Input.GetMouseButton(0)) {
            if (Physics.Raycast(shootPos.transform.position + shootPos.transform.forward, shootPos.transform.forward, out hit, Mathf.Infinity)) {
                //Debug.DrawRay(shootPos.transform.position + shootPos.transform.forward, shootPos.transform.forward * hit.distance, Color.black, Mathf.Infinity);
                if (hit.collider.gameObject.tag.Equals("Enemy")) {
                    Destroy(hit.collider.gameObject);

                }

            }
        }
    }

    
}
