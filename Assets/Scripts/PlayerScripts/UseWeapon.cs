using JetBrains.Annotations;
using NUnit;
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
    public PlayerCamera playerCamera;

    public Vector3 recoilValue;
    public Vector3 rotationRecoilValue;

    public Vector3 cameraRecoilValue;
    public Vector3 cameraRotationalRecoilValue;

    public GameObject hand;
    private VMEffects handRecoil;
    public String handName;
    private bool isRightHand;
    private bool mouseDown;

    public float damage;
    public GameObject weapon;

    //private Vector3[] positions = new Vector3[2];

    // Start is called before the first frame update
    void Start() {
        recoilForce = GetComponent<VMEffects>();
        handRecoil = hand.GetComponent<VMEffects>();
        isRightHand = handName.Equals("right arm");
        attackCooldown = 0;
        canAttack = false;

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
        
        handRecoil.applyForce(cameraRecoilValue, 5, rotationRecoilValue);
        playerCamera.applyCameraForce(cameraRecoilValue, cameraRotationalRecoilValue);

        LineRenderer newTracer = Instantiate(tracer, shootPos.transform);

        Vector3[] positions = new Vector3[2];

        positions[0] = start;
        positions[1] = end;
        newTracer.positionCount = 2;
        newTracer.SetPositions(positions);

        yield return new WaitForSeconds(0.15f);
        Debug.Log("coroutine ended???");
        Destroy(newTracer);
    }

    IEnumerator CreateTracer(Vector3 start, Vector3 end) {
        GameObject newTracerPosition = Instantiate(shootPos, shootPos.transform.position, shootPos.transform.rotation, shootPos.transform);
        LineRenderer newTracer = newTracerPosition.GetComponent<LineRenderer>();

        Vector3[] positions = new Vector3[2];

        positions[0] = start;
        positions[1] = end;
        newTracer.positionCount = 2;
        newTracer.SetPositions(positions);
        yield return new WaitForSeconds(0.15f);
        Destroy(newTracer);

        yield return null;
    }

    
}
