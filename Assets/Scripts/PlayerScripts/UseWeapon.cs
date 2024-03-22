using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

public class FireWeapon : MonoBehaviour
{
    private GameObject weapon;
    private GameObject VM;
    private GameObject shootPos;
    private RaycastHit hit;
    private GameObject enemy;

    private LineRenderer tracer;

    private bool canAttack;
    public float attackSpeed;
    private float attackCooldown;

    private bool attacking;

    public  Camera playerCamera;
    private VMEffects recoilForce;

    private Vector3[] positions = new Vector3[2];

    // Start is called before the first frame update
    void Start()
    {
        weapon = GameObject.Find("weapon");
        VM = GameObject.Find("ViewModel");
        shootPos = GameObject.Find("ShootPos");
        recoilForce = GetComponent<VMEffects>();
        
        tracer = shootPos.GetComponent<LineRenderer>();
        tracer.enabled = false;
        tracer.positionCount = 2;
        tracer.startWidth = 0.25f;
        tracer.endWidth = 0.25f;

    }

    private void Update() {

    }

    private void FixedUpdate() {

        if (attackCooldown >= attackSpeed) {
            attackCooldown = 0;
            canAttack = true;

        } else {
            attackCooldown += Time.deltaTime;

        }

        if (Input.GetMouseButton(0))
        {
            if (canAttack) {
                StartCoroutine(UseWeapon());

            }
        }
    }

    IEnumerator UseWeapon() {

        if (canAttack) {
            positions[0] = shootPos.transform.position;

            tracer.enabled = true;
            tracer.positionCount = 2;

            if (Physics.Raycast(shootPos.transform.position + shootPos.transform.forward, shootPos.transform.forward, out hit, Mathf.Infinity)) {
                //Debug.DrawRay(shootPos.transform.position + shootPos.transform.forward, shootPos.transform.forward * hit.distance, Color.black);
                if (hit.collider.gameObject.tag.Equals("Enemy"))
                {
                    enemy = hit.collider.gameObject;
                    enemy.GetComponent<Enemy>().takeDamage(3);

                }

                positions[1] = hit.point;
            }
            else {
                positions[1] = shootPos.transform.position + shootPos.transform.forward * 100;

            }

            recoilForce.applyForce(new Vector3(0, 0f, -0.25f), 20);

            attacking = true;

            tracer.SetPositions(positions);
            canAttack = false;
            yield return new WaitForSeconds(0.15f);
            recoilForce.applyForce(new Vector3(0, 0f, 0.25f), 3);

            tracer.enabled = false;
            attacking = false;
        }
    }

    public bool isFiring() {
        return attacking;
    }

    
}
