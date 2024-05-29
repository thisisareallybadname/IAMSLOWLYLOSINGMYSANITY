using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAI : MonoBehaviour {

    public GameObject player;
    public int walkspeed;
    public Rigidbody rb;

    public bool canFireProjectiles;
    private bool chasePlayer;

    public GameObject projectile;
    public float projectileFirerate;
    private float fireCooldown = 0;

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void FixedUpdate() {
        transform.LookAt(player.transform.position);

        if (chasePlayer) {
            rb.AddForce(transform.forward * walkspeed * 200 * Time.deltaTime, ForceMode.Force);
            rb.drag = 2.5f;

            rb.velocity += walkspeed * (rb.transform.forward.normalized / 7.5f);
            
            if (fireCooldown >= projectileFirerate && canFireProjectiles) {
                FireProjectile();
                fireCooldown = 0;
            } else {
                fireCooldown += Time.fixedDeltaTime;

            }

        }
    }

    private void FireProjectile() {
        GameObject newFireball = Instantiate(projectile, transform.position + transform.forward, transform.rotation);
        ProjectileBehavior fireballProperties = newFireball.GetComponent<ProjectileBehavior>();
        fireballProperties.canExplode = true;
        fireballProperties.walkspeed = 10;
        fireballProperties.dangerous = true;
    }

    public void setMovementSpeed(float newWalkspeed) {

        walkspeed = (int)newWalkspeed;
    }

    public void MakeHostile(bool chase) {
        chasePlayer = chase;
    }

    

}
