using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour {

    public float walkspeed;
    public float timer;

    private Collider[] hits = new Collider[0];
    private GameObject player;
    private GameObject explosionEffect;

    private bool touchingSomething;

    private bool explosionExpanding;
    private float countdown = 0;

    public bool dangerous;

    // Start is called before the first frame update
    void Start() {
        player = GameObject.Find("Player");
        explosionEffect = Instantiate(GameObject.Find("explosion"), transform.position, Quaternion.identity);
        explosionEffect.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        explosionEffect.transform.parent = transform;
        explosionEffect.transform.localPosition = Vector3.zero;

        transform.LookAt(player.transform.position);

    }

    // Update is called once per frame
    void FixedUpdate() {
        if (explosionExpanding) {
            if (countdown > 1) {
                Destroy(explosionEffect);
                Destroy(transform.gameObject);

            } else {
                explosionEffect.transform.localScale = Vector3.Lerp(explosionEffect.transform.localScale, new Vector3(15, 15, 15), countdown);
                countdown += Time.fixedDeltaTime;

            }
            
        }

    }

    private void OnTriggerEnter(Collider collision) {
        if (!collision.gameObject.tag.Equals("floor") && !collision.gameObject.tag.Equals("bomb") && dangerous) {
            hits = Physics.OverlapSphere(transform.position, 15);

            foreach (Collider collider in hits)
            {
                if (collider.gameObject.tag.Equals("Player"))
                {
                    player.GetComponent<PlayerDamage>().takeDamage(4);
                    touchingSomething = true;

                }
                else if (collider.gameObject.tag.Equals("Enemy"))
                {
                    collider.gameObject.GetComponent<EnemyHealth>().takeDamage(99999999999, 1);
                    touchingSomething = true;

                }

                explosionExpanding = true;

            }
        }
    }
}
