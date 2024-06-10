using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// script that controls the enemy's AI
public class EnemyMovement : MonoBehaviour {

    [SerializeField] GameObject player;
    [SerializeField] float walkspeed;
    private Rigidbody rb; // enemy's rigidbody

    [SerializeField] bool canFireProjectiles;

    [SerializeField] GameObject projectile;

    [SerializeField] float projectileSpeed; // speed projectile is travelling at
    [SerializeField] float projectileFirerate = 1;

    [SerializeField] float projectileFirerateVariation; // add variation between each enemy's ROF so it wont be too easy to dodge
    private float fireCooldown = 0; // tiner for keeping track of rate of fire

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

    // modify the projectile's stats
    // three modes that can be used, multiply stats, add stats, or just set them (default)
    public void ModifyProjectileVariables(float speed, float firerate, float damage, string mode) {
        if (mode.Equals("multi"))
        {
            projectileFirerate *= firerate;
            projectileSpeed *= speed;

        }
        else if (mode.Equals("add")) {
            projectileFirerate += firerate;
            projectileSpeed += speed;

        } else {
            projectileFirerate = firerate;
            projectileSpeed = speed;

        }

        projectile.GetComponent<ProjectileBehavior>().setDamage(damage, mode);
    }

    // make a new enemyprojectile object
    private void FireProjectile() {

        // nake fireball object
        GameObject newFireball = Instantiate(projectile, transform.position + transform.forward, transform.rotation);
        ProjectileBehavior fireballProperties = newFireball.GetComponent<ProjectileBehavior>();

        // by default the projectile's behavior is disabled so it won't accidentally blow up
        fireballProperties.enabled = true;
        fireballProperties.setMovespeed(projectileSpeed, "set");
    }

    // set the movespeed of the enemy
    public void setWalkspeed(float newWalkspeed, string mode) {
        if (mode.Contains("mult")) {
            walkspeed *= newWalkspeed;

        } else if (mode.Equals("add")) {
            walkspeed += newWalkspeed;

        } else
            walkspeed = newWalkspeed;
    }
    
}
