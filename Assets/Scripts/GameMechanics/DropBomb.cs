using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LandmineSetter : MonoBehaviour {

    public WaveManager manager;
    private float bombCounter;
    public float bombLimit;

    private bool canDropBombs;

    public float floorX;
    public float floorZ;

    public GameObject landmine;
    
    private HashSet<GameObject> landmines = new HashSet<GameObject>();

    // Start is called before the first frame update
    
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        canDropBombs = manager.intermission;

        bombLimit = manager.wave * 5;

        if (canDropBombs) {
            if (bombCounter < bombLimit) {
                transform.position = new Vector3(Random.Range(-floorX / 2, floorX / 2), 3, Random.Range(-floorZ / 2, floorZ / 2));
                GameObject newLandmine = Instantiate(landmine, transform.position, Quaternion.identity);
                newLandmine.GetComponent<ProjectileBehavior>().dangerous = true;

                landmines.Add(newLandmine);

                bombCounter++;

            }

        } else {
            bombCounter = 0;

                foreach (GameObject landmine in landmines) {
                    Destroy(landmine);

                }

            
            
        }
    }

}
