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

    // Start is called before the first frame update
    void Start() {
        player = GameObject.Find("Player");
        transform.LookAt(player.transform.position);

    }

    // Update is called once per frame
    void FixedUpdate() {


    }

    IEnumerator fadeOut() {
        //explosionEffect.transform.localScale = Vector3.Lerp(explosionEffect.transform.localScale, Vector3.zero, 5);
        yield return new WaitForSeconds(1);

        Destroy(explosionEffect);
        Destroy(transform.gameObject);


    }

    private void OnTriggerEnter(Collider collision) {
        if (!collision.gameObject.tag.Equals("floor")) {
            hits = Physics.OverlapSphere(transform.position, 15);

            explosionEffect = Instantiate(GameObject.Find("explosion"), transform.position, Quaternion.identity);
            explosionEffect.transform.localScale = new Vector3(15, 15, 15);

            foreach (Collider collider in hits)
            {
                if (collider.gameObject.tag.Equals("Player"))
                {
                    player.GetComponent<PlayerDamage>().setHealth(player.GetComponent<PlayerDamage>().health - 3);
                    touchingSomething = true;

                }
                else if (collider.gameObject.tag.Equals("Enemy"))
                {
                    collider.gameObject.GetComponent<EnemyHealth>().takeDamage(99999999999, 1);
                    touchingSomething = true;

                }

                StartCoroutine(fadeOut());

            }
        }
    }
}
