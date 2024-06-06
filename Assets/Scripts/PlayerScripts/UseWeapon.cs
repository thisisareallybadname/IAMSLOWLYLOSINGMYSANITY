using JetBrains.Annotations;
using NUnit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

public class FireWeapon : MonoBehaviour
{
    [SerializeField] GameObject shootPos;
    private RaycastHit hit;
    private GameObject enemy;

    [SerializeField] LineRenderer tracer;

    private bool canAttack = false;
    [SerializeField] float attackSpeed;
    private float attackCooldown;

    private bool attacking;

    private float originalDamage;
    private float originalFirerate;

    [SerializeField] PlayerCamera playerCamera;

    [SerializeField] Vector3 VMrecoil;
    [SerializeField] Vector3 VMAngleRecoil;

    [SerializeField] Vector3 cameraRecoil;
    [SerializeField] Vector3 cameraAngleRecoil;

    [SerializeField] VMEffects handRecoil;
    [SerializeField] bool isRightHand;
    private bool mouseDown;

    [SerializeField] float damage;
    [SerializeField] GameObject weapon;

    private List<LineRenderer> listOfTracers;

    //private Vector3[] positions = new Vector3[2];

    // Start is called before the first frame update
    void Start() {
        handRecoil = GetComponent<VMEffects>();
        attackCooldown = 0;
        canAttack = false;

        originalDamage = damage;
        originalFirerate = attackSpeed;

        listOfTracers = new List<LineRenderer>();

        if (isRightHand) {
            VMrecoil = new Vector3(-VMrecoil.x, VMrecoil.y, -VMrecoil.z);
            cameraAngleRecoil = 
            new Vector3(cameraAngleRecoil.z, cameraAngleRecoil.y, cameraAngleRecoil.x);

        }

    }

    private void Update() {
        if (isRightHand) {
            mouseDown = Input.GetMouseButton(1);

        } else {
            mouseDown = Input.GetMouseButton(0);

        }

    }

    private void FixedUpdate() {

        if (attackCooldown >= attackSpeed) {
            attackCooldown = 0;
            canAttack = true;

        }
        else
        {
            attackCooldown += Time.fixedDeltaTime;

        }

        if (mouseDown) {
            if (canAttack) {
                canAttack = false;
                StartCoroutine(UseWeapon());

            }
        }
    }

    public bool isFiring() {
        return attacking;
    }

    public void resetWeaponStats() {
        damage = originalDamage;
        attackSpeed = originalFirerate;

    }

    public void SetWeaponStats(float newDamage, float newFirerate, bool multi) {
        if (multi) {
            damage *= newDamage;
            attackSpeed *= newFirerate;

        } else {
            damage = newDamage;
            attackSpeed = newFirerate;

        }

    }

    public float getDamage() {
        return damage;

    }

    IEnumerator UseWeapon() {
        canAttack = false;
        Vector3 origin = shootPos.transform.position + shootPos.transform.forward;
        Vector3 direction = shootPos.transform.forward;
        Vector3 endPoint;

        // fire raycast in front of player's shoot position
        if (Physics.Raycast(origin, direction, out hit, Mathf.Infinity)) {
            if (hit.collider.gameObject.tag.Contains("Enemy")) {
                enemy = hit.collider.gameObject;
                enemy.GetComponent<EnemyHealth>().takeDamage(damage, damage * 2f);

            } else if (hit.collider.gameObject.tag.Equals("bomb")) {
                hit.collider.gameObject.GetComponent<ProjectileBehavior>().explode();

            }

            endPoint = hit.point;

        } else {
            endPoint = shootPos.transform.position + shootPos.transform.forward * 150;

        }
        
        handRecoil.applyForce(VMrecoil, 5, VMAngleRecoil);

        if (isRightHand) {
            playerCamera.applyCameraForce(cameraRecoil, cameraAngleRecoil);
        }

        StartCoroutine(CreateTracer(tracer, origin, endPoint));
        yield return null;
    }

    IEnumerator CreateTracer(LineRenderer tracer, Vector3 start, Vector3 end) {
        LineRenderer newTracer = Instantiate(tracer, start, Quaternion.identity);

        Vector3[] positions = new Vector3[2];

        positions[0] = start;
        positions[1] = end;

        newTracer.positionCount = 2;
        newTracer.SetPositions(positions);
        listOfTracers.Add(newTracer);
        yield return new WaitForSeconds(0.15f);

        newTracer.enabled = false;

        Destroy(listOfTracers[0]);
        listOfTracers.RemoveAt(0);
    }
}
