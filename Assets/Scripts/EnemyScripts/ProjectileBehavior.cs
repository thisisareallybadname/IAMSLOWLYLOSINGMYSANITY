using JetBrains.Annotations;
using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEditor.DeviceSimulation;
using UnityEngine;

// hazard for player, either moves as a projectile or stands stationary as a landmine
public class ProjectileBehavior : MonoBehaviour {

    [SerializeField] float walkspeed; // projectile movespeed
    [SerializeField] float timer;

    private Collider[] hits = new Collider[0];
    private GameObject player;
    private GameObject explosionEffect; // the orange circle
    private GameObject TransparentExplosionEffect; // the slightly bigger, slightly translucent orange circle

    private float countdown;

    private bool explosionExpanding;

    [SerializeField] float damage; 
    [SerializeField] bool canExplode; // true if projectile causes explosion effect
    private bool exploded = false;

    [SerializeField] float explosionRadius; // radius of explosion

    [SerializeField] float lifespan; // how long projectile lasts
    private float existenceTimer;

    private Rigidbody rb;

    private Vector3 size;
    private bool lopsided;

    private Vector3 targetSize; // max size for explosion effect

    // Start is called before the first frame update
    void Awake() {


        size = transform.localScale;
        lopsided = !(size.x == size.y && size.y == size.z);
        exploded = false;

        player = GameObject.Find("Player");

        if (canExplode) {
            explosionEffect = Instantiate(GameObject.Find("explosion"), transform.position, Quaternion.identity);
            explosionEffect.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            explosionEffect.transform.parent = transform;
            explosionEffect.transform.localPosition = Vector3.zero;

            TransparentExplosionEffect = explosionEffect.transform.GetChild(0).gameObject;

        }
        
        rb = GetComponent<Rigidbody>();

        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

        if (lopsided)
        { // lopsided = transform scale isn't a cube
            targetSize =
            new Vector3(explosionRadius, explosionRadius * 2, explosionRadius) / 2;
        }
        else
        {
            targetSize = Vector3.one * explosionRadius;

        }

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

        // cause explosion effect to grow
        if (explosionExpanding) {

            // if explosion effect reaches certain size, delete stuff
            if (countdown >= 0.5f) {
                Destroy(explosionEffect);
                Destroy(transform.gameObject);

            } else {
                if (canExplode) {

                    // lerp explosion effect
                    explosionEffect.transform.localScale = 
                    Vector3.Lerp(Vector3.one / 4, targetSize, countdown * 2.5f);
                    TransparentExplosionEffect.transform.localScale = 
                    Vector3.Lerp(Vector3.one * 1.25f, Vector3.one * 1.75f, countdown * 2.5f);

                }
                countdown += Time.fixedDeltaTime;

            }
            
        }

    }
       
    // explode if it touches something thats not a bomb/floor
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

    // set projectile movespeed
    public void setMovespeed(float speed, string mode) {
        if (mode.Contains("mult")) {
            walkspeed *= speed;

        } else if (mode.Equals("add")) {
            walkspeed += speed;

        } else {
            walkspeed = speed;

        }

    }

    public bool projectileExploded() {
        print("boom");
        return exploded;

    }

    public void setDamage(float newDamage, string mode) {
        if (mode.Contains("mult")) {
            damage *= newDamage;

        } else if (mode.Equals("add")) {
            damage += newDamage;

        } else {
            damage = newDamage;

        }

    }

    // explode, the selling point of the landmine
    public void explode() {
        if (!exploded) {
            exploded = true;

            // deal damage to all entities in radius
            hits = Physics.OverlapSphere(transform.position, explosionRadius / 4);

            // freeze landmine in place so explosion isnt affected by gravity
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;


            // affect everything in radius
            foreach (Collider collider in hits) {

                // deal dmg to player
                if (collider.gameObject.tag.Equals("Player")) {
                    player.GetComponent<PlayerDamage>().takeDamage(damage);

                }

                // deal dmg to enemy
                else if (collider.gameObject.tag.Equals("Enemy")) {
                    collider.gameObject.GetComponent<EnemyHealth>().takeDamage(damage, damage * 3);

                // explode other bombs
                } else if (collider.gameObject.tag.Equals("bomb")) {
                    exploded = true;
                    collider.gameObject.GetComponent<ProjectileBehavior>().explode();

                }

                // used to start explosion grow thingy
                explosionExpanding = true;

            }
        }
    }
}
