using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// script that controls the enemy's AI
public class enemyAI : MonoBehaviour {

    public GameObject player;
    public int walkspeed; 
    public Rigidbody rb;

    public bool canFireProjectiles;

    public GameObject projectile;

    public float projectileSpeed;
    public float projectileFirerate = 1;
    public float projectileFirerateVariation;
    private float fireCooldown = 0;

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
    public void setMovementSpeed(float newWalkspeed) {

        walkspeed = (int)newWalkspeed;
    }
}
