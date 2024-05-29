using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEditor.DeviceSimulation;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour {

    public float walkspeed;
    public float timer;

    private Collider[] hits = new Collider[0];
    private GameObject player;
    private GameObject explosionEffect;
    private float playerWalkspeed;

    private bool touchingSomething;
    private float countdown;

    private bool explosionExpanding;
    private float aimingTimer = 0;

    public bool dangerous;
    public float damage;
    public bool canExplode;
    private bool exploded = false;

    public float explosionRadius;
    private bool deleteAfterExplosion;

    public float lifespan;
    private float existenceTimer;

    public float aimTime;

    // Start is called before the first frame update
    void Awake() {
        player = GameObject.Find("Player");
        playerWalkspeed = player.GetComponent<PlayerMovement>().walkspeed;

        if (canExplode) {
            explosionEffect = Instantiate(GameObject.Find("explosion"), transform.position, Quaternion.identity);
            explosionEffect.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            explosionEffect.transform.parent = transform;
            explosionEffect.transform.localPosition = Vector3.zero;

        } else {
            explosionRadius = 1.5f;

        }
        deleteAfterExplosion = false;
        PlayerMovement movement = player.GetComponent<PlayerMovement>();

    }

    // Update is called once per frame
    void FixedUpdate() { 
        if (aimingTimer < aimTime && walkspeed > 0) {
            aimingTimer += Time.fixedDeltaTime;
            GetComponent<Rigidbody>().drag = 15;
            transform.LookAt(player.transform);

        }

        if (dangerous && walkspeed > 0 && GetComponent<Rigidbody>() != null) {
            GetComponent<Rigidbody>().AddForce((transform.forward) * walkspeed, ForceMode.Impulse);

            if (existenceTimer < lifespan)
            {
                existenceTimer += Time.fixedDeltaTime;

            } else {
                Destroy(gameObject);

            }
        }

        if (explosionExpanding) {
            if (countdown >= 1) {
                Destroy(explosionEffect);
                Destroy(transform.gameObject);

            } else {
                if (canExplode) {
                    Vector3 landmineLocalScale = transform.localScale;
                    explosionEffect.transform.localScale = Vector3.Lerp(new Vector3(0.25f, 0.25f, 0.25f), new Vector3(explosionRadius * 2, explosionRadius * 4, explosionRadius * 2), countdown * 2.5f);
                }
                countdown += Time.fixedDeltaTime;

            }
            
        }

    }

    private void OnTriggerStay(Collider collision) {
        if (!collision.gameObject.tag.Equals("floor") && !collision.gameObject.tag.Equals("bomb") && dangerous) {
            explode();
        }
    }

    public void diffuse() {
        if (!explosionExpanding) {
            Destroy(this.gameObject);

        }

    }

    public void explode() {
        if (!exploded) {
            exploded = true;
            hits = Physics.OverlapSphere(transform.position, explosionRadius);

            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;

            foreach (Collider collider in hits)
            {

                if (collider.gameObject.GetComponent<Rigidbody>() != null) {
                    collider.gameObject.GetComponent<Rigidbody>().AddForce((collider.gameObject.transform.position - transform.position) * 5, ForceMode.Impulse);

                }

                if (collider.gameObject.tag.Equals("Player")) {
                    player.GetComponent<PlayerDamage>().takeDamage(damage);
                    touchingSomething = true;

                }
                else if (collider.gameObject.tag.Equals("Enemy")) {
                    collider.gameObject.GetComponent<EnemyHealth>().takeDamage(damage, 25);
                    touchingSomething = true;

                } else if (collider.gameObject.tag.Equals("bomb")) {
                    exploded = true;
                    collider.gameObject.GetComponent<ProjectileBehavior>().explode();

                }

                explosionExpanding = true;

            }
        }
    }
}
