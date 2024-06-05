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

    public float damage;
    public bool canExplode;
    private bool exploded = false;

    public float explosionRadius;
    private bool deleteAfterExplosion;

    public float lifespan;
    private float existenceTimer;

    public float aimTime;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Awake() {

        exploded = false;

        player = GameObject.Find("Player");
        playerWalkspeed = player.GetComponent<PlayerMovement>().walkspeed;

        if (canExplode) {
            explosionEffect = Instantiate(GameObject.Find("explosion"), transform.position, Quaternion.identity);
            explosionEffect.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            explosionEffect.transform.parent = transform;
            explosionEffect.transform.localPosition = Vector3.zero;

        }
        
        rb = GetComponent<Rigidbody>();

        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

        transform.LookAt(player.transform);
    }

    // Update is called once per frame
    void FixedUpdate() { 

        if (walkspeed > 0 && rb != null) {
            rb.AddForce((transform.forward) * walkspeed, ForceMode.Impulse);
            if (rb.velocity.magnitude > walkspeed) {
                rb.velocity = rb.velocity.normalized * walkspeed;

            }

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
                    if (walkspeed == 0) {
                        explosionEffect.transform.localScale = Vector3.Lerp(new Vector3(0.25f, 0.25f, 0.25f), new Vector3(explosionRadius, explosionRadius * 2, explosionRadius), countdown * 4.5f);
                    
                    } else {
                        explosionEffect.transform.localScale = Vector3.Lerp(new Vector3(0.25f, 0.25f, 0.25f), new Vector3(explosionRadius, explosionRadius, explosionRadius ), countdown * 4.5f);

                    }
                }
                countdown += Time.fixedDeltaTime;

            }
            
        }

    }

    private void OnTriggerStay(Collider collision) {
        if (!collision.gameObject.tag.Equals("floor") && !collision.gameObject.tag.Equals("bomb")) {
            explode();
        }
    }

    public void diffuse() {
        if (!explosionExpanding) {
            Destroy(this.gameObject);

        }

    }

    public void enableLandmine() {
        enabled = true;

    }

    public bool projectileExploded() {
        print("boom");
        return exploded;

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
