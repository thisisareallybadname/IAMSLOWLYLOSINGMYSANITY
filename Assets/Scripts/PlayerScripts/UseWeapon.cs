using JetBrains.Annotations;
using NUnit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

// lets player fight enemies with weapons
public class FireWeapon : MonoBehaviour {

    // position where raycast will start
    [SerializeField] GameObject shootPos;
    private RaycastHit hit;
    private GameObject enemy;

    [SerializeField] LineRenderer tracer;

    private bool canAttack = false;
    [SerializeField] float attackSpeed;
    private float attackCooldown;

    // used to reset dmg
    private float originalDamage;
    private float originalFirerate;

    [SerializeField] PlayerCamera playerCamera;

    // recoil fields
    [SerializeField] Vector3 VMrecoil;
    [SerializeField] Quaternion VMAngleRecoil;

    // camera recoil fields
    [SerializeField] Vector3 cameraRecoil;
    [SerializeField] Quaternion cameraAngleRecoil;

    [SerializeField] VMEffects handRecoil;
    [SerializeField] bool isRightHand;

    // used to determine if player is holding correct mouse button down
    private bool mouseDown;

    [SerializeField] float damage;

    // Start is called before the first frame update
    void Start() {

        // set important private variables above
        handRecoil = GetComponent<VMEffects>();
        attackCooldown = 0;
        canAttack = false;

        originalDamage = damage;
        originalFirerate = attackSpeed;

        // if hand is the right hand, alternate the thingy
        if (isRightHand) {
            VMrecoil = new Vector3(-VMrecoil.x, VMrecoil.y, -VMrecoil.z); 
            cameraAngleRecoil = 
            Quaternion.Euler(cameraAngleRecoil.z, cameraAngleRecoil.y, cameraAngleRecoil.x);

        }
        VMAngleRecoil = 
        Quaternion.Euler(VMAngleRecoil.x / 10, VMAngleRecoil.y / 10, VMAngleRecoil.z / 10);
        cameraAngleRecoil =  Quaternion.Euler
        (cameraAngleRecoil.x / 10, cameraAngleRecoil.y / 10, cameraAngleRecoil.z / 10);

    }

    private void FixedUpdate() {

        if (attackCooldown >= attackSpeed) {
            canAttack = true;

        } else {
            attackCooldown += Time.fixedDeltaTime;

        }

        // mousedown for arm is true if the corresponding mouse button is pressed
        // ex: mousedown is true for script in left arm when LMB is pressed but is false if 
        // LMB is pressed in right hand
        if (isRightHand) {
            mouseDown = Input.GetMouseButton(1);

        } else {
            mouseDown = Input.GetMouseButton(0);

        }

        // fire weapon
        if (mouseDown) {
            if (canAttack) {
                canAttack = false;

                attackCooldown = 0;
                StartCoroutine(UseWeapon());

            }
        }
    }

    public void resetWeaponStats() {
        damage = originalDamage;
        attackSpeed = originalFirerate;

    }

    // set weapon firerate and damage
    // identical to the other setters/getters (aside from variant index one)
    // mode is pretty self explanatory
    // first thing in if statement uses contains() because im incredibly inconsistent
    public void SetWeaponStats(float newDamage, float newFirerate, string mode) {
        if (mode.Contains("mult")) {
            damage *= newDamage;
            attackSpeed *= newFirerate;

        } else if (mode.Equals("add")) {
            damage += newDamage;
            attackSpeed += newFirerate;

        } else {
            damage = newDamage;
            attackSpeed = newFirerate;

        }

    }

    // fire raycast and create tracer
    IEnumerator UseWeapon() {
        canAttack = false;
        Vector3 origin = shootPos.transform.position + shootPos.transform.forward;
        Vector3 direction = shootPos.transform.forward;
        Vector3 endPoint;

        // fire raycast in front of player's shoot position
        if (Physics.Raycast(origin, direction, out hit, Mathf.Infinity)) {
            if (hit.collider.gameObject.tag.Contains("Enemy")) {
                enemy = hit.collider.gameObject;

                //Destroy(enemy);

                enemy.GetComponent<EnemyHealth>().takeDamage(damage, damage * 2f);

            } else if (hit.collider.gameObject.tag.Equals("bomb")) {
                hit.collider.gameObject.GetComponent<ProjectileBehavior>().explode();

            }

            endPoint = hit.point;

        } else {
            endPoint = shootPos.transform.position + shootPos.transform.forward * 150;

        }
        
        // apply recoil effects
        handRecoil.applyForce(VMrecoil, 5, VMAngleRecoil);
        playerCamera.applyCameraForce(cameraRecoil, cameraAngleRecoil);

        StartCoroutine(CreateTracer(tracer, origin, endPoint));
        yield return null;
    }

    // create a tracer
    IEnumerator CreateTracer(LineRenderer tracer, Vector3 start, Vector3 end) {
        LineRenderer newTracer = Instantiate(tracer, start, Quaternion.identity);

        // array needed to set start and end points of tracer
        // first element is start, last element is end
        Vector3[] positions = new Vector3[2];

        positions[0] = start;
        positions[1] = end;

        newTracer.positionCount = 2;

        // change width of tracer depending on damage
        newTracer.startWidth = damage / 10f;
        newTracer.endWidth = damage / 10f;

        newTracer.SetPositions(positions);
        yield return new WaitForSeconds(0.075f);

        newTracer.enabled = false;

        // here we go again because unity is doing a funny and not deleting listoftracers[0]
        Destroy(GameObject.Find(newTracer.name));
    }
}
