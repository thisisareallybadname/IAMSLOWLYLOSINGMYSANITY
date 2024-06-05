using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// script that controls the enemy's AI
public class enemyAI : MonoBehaviour {

    [SerializeField] GameObject player;
    [SerializeField] float walkspeed; 
    private Rigidbody rb;

    public bool canFireProjectiles;

    public GameObject projectile;

    [SerializeField] float projectileSpeed;
    [SerializeField] float projectileFirerate = 1;
    [SerializeField] float projectileFirerateVariation;
    private float fireCooldown = 0;

    private void Start() {
        rb = GetComponent<Rigidbody>();

    }

    // Start is called before the first frame update
    void Awake() {
        projectileFirerate *= 1 + Random.Range(-projectileFirerateVariation, projectileFirerateVariation) / 50f;
    }

    void EnableRangedAttack() {
        canFireProjectiles = true;

    }

    // Update is called once per frame
    void FixedUpdate() {
        gameObject.transform.LookAt(player.transform); // look at the player

            // move rigidbody to position in front of enemy, so it will go towards player
            rb.AddForce(transform.forward * walkspeed * 200 * Time.deltaTime, ForceMode.Force);
            rb.drag = 1.25f;

            // a little bit of acceleration because why not
            rb.velocity += walkspeed * (rb.transform.forward.normalized / 7.5f);

            // if the enemy is a ranged enemy, fire a projectile 
            if (canFireProjectiles) {
                if (fireCooldown >= projectileFirerate) {
                    FireProjectile();
                    fireCooldown = 0;
    
                } else {
                    fireCooldown += Time.fixedDeltaTime;
    
                }
            }
    }

    public void ModifyProjectileVariables(float newSpeed, float newFirerate, bool multiply) {
        if (multiply) {
            projectileFirerate *= newFirerate;
            projectileSpeed *= newSpeed;

        } else {
            projectileFirerate = newFirerate;
            projectileSpeed = newSpeed;

        }
    }

    // make a new enemyprojectile object
    private void FireProjectile() {

        // nake fireball object
        GameObject newFireball = Instantiate(projectile, transform.position + transform.forward, transform.rotation);
        ProjectileBehavior fireballProperties = newFireball.GetComponent<ProjectileBehavior>();
        
        // by default the projectile's behavior is disabled so it won't accidentally blow up
        fireballProperties.enabled = true;
        fireballProperties.canExplode = true;
        fireballProperties.walkspeed = projectileSpeed;
    }

    // set the movespeed of the enemy
    public void setWalkspeed(float newWalkspeed) {

        walkspeed = newWalkspeed;
    }
}
