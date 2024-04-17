using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringModule : MonoBehaviour {

    private Vector3 force;
    private Vector2 force2D;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public SpringModule() {
        force = Vector3.zero;

    }

    IEnumerator updateSpring3D(Vector3 v, float forceTime, string springType) {
        float i = 0;
        while (i < forceTime) {
            if (springType.Equals("rotation")) {
                force = Vector3.Slerp(v, Vector3.zero, i);
            
            } else {
                force = Vector3.Lerp(v, Vector3.zero, i);

            }
            i += Time.fixedDeltaTime;
        }

        yield return null;
    }

    IEnumerator updateSpring2D(Vector2 v, float forceTime, string springType) {
        float i = 0;
        while (i < forceTime) {
            if (springType.Equals("rotation")) {
                force = Vector3.Slerp(v, Vector3.zero, i);

            } else {
                force = Vector3.Lerp(v, Vector3.zero, i);

            }
            i += Time.fixedDeltaTime;
        }

        yield return null;
    }

    public void shove3D(Vector3 force, float forceSpeed, string springType) {
        StartCoroutine(updateSpring3D(force, forceSpeed, springType));
    }

    public void shove2D(Vector2 force, float forceSpeed, string springType) {
        StartCoroutine(updateSpring2D(force, forceSpeed, springType));

    }

    public Vector3 getForce3D() {
        return force;

    }

    public Vector2 getForce2D() {
        return force2D;

    }
}
