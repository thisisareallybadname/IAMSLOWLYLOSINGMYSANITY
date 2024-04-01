using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

public class FireWeapon : MonoBehaviour
{
    public GameObject shootPos;
    private RaycastHit hit;
    private GameObject enemy;


    public LineRenderer tracer;

    private bool canAttack;
    public float attackSpeed;
    private float attackCooldown;

    private bool attacking;

    private VMEffects recoilForce;

    public GameObject hand;
    private VMEffects handRecoil;
    public String handName;
    private bool isRightHand;
    private bool mouseDown;

    public float damage;
    public GameObject weapon;

    private Vector3[] positions = new Vector3[2];

    // Start is called before the first frame update
    void Start()
    {
        recoilForce = GetComponent<VMEffects>();
        
        tracer.enabled = false;
        tracer.positionCount = 2;
        tracer.startWidth = 0.25f;
        tracer.endWidth = 0.25f;

        handRecoil = hand.GetComponent<VMEffects>();
        isRightHand = handName.Equals("right arm");

    }

    private void Update() {
        if (isRightHand) {
            mouseDown = Input.GetMouseButton(1);

        } else {
            mouseDown = Input.GetMouseButton(0);

        }

        if (attackCooldown >= attackSpeed) {
            attackCooldown = 0;
            canAttack = true;

        } else {
            attackCooldown += Time.deltaTime;

        }
    }

    private void FixedUpdate() {

        if (mouseDown) {
            if (canAttack) {
                StartCoroutine(UseWeapon());

            }
        }

        //weapon.transform.localPosition = hand.transform.localPosition + new Vector3(0, 0, 1);
    }

    IEnumerator MakeTracer(Vector3 start, Vector3 end) {

        
            positions[0] = start;
            tracer.positionCount = 2;
            tracer.enabled = true;
            tracer.SetPositions(positions);
            yield return new WaitForSeconds(0.15f);
            tracer.enabled = false;
        
    }

    public bool isFiring() {
        return attacking;
    }

    IEnumerator UseWeapon() {
        canAttack = false;
        Vector3 start = shootPos.transform.position;
        Vector3 end;

        if (Physics.Raycast(shootPos.transform.position + shootPos.transform.forward, shootPos.transform.forward, out hit, Mathf.Infinity)) {
            if (hit.collider.gameObject.tag.Equals("Enemy")) {
                enemy = hit.collider.gameObject;
                enemy.GetComponent<Enemy>().takeDamage(damage);

            }

            end = hit.point;

        } else {
            end = shootPos.transform.position + shootPos.transform.forward * 100;

        }
        positions[0] = start;
        positions[1] = end;

        tracer.SetPositions(positions);
        tracer.enabled = true;
        recoilForce.applyForce(new Vector3(0, 0.15f, -0.5f), 5, new Vector3(-35f, 15, 0));

        yield return new WaitForSeconds(0.1f);
        tracer.enabled = false;
    }

    
}
