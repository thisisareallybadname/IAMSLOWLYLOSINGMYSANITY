using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

// sets up the landmine positions and stuff in the field
// arguably the ugliest used script in this entire project
public class LandmineSetter : MonoBehaviour {

    // fields that store the amount of bombs per wave 
    private float bombCounter;
    public float bombLimit;

    private bool canDropBombs;
    private bool spawnIndicators;

    // the floor's width along the x axis and z axis
    public float floorX;
    public float floorZ;

    // the landmine, big boom when touched
    public GameObject landmine;

    // the landmine indicator, tells the player where the landmines are before the wave starts
    public GameObject landmineIndicator;

    public bool canSpawnBombs;

    private List<Vector3> landmineSpawns = new List<Vector3>(); // the actual positions where the landmines will be at
    private List<GameObject> landmines = new List<GameObject>();
    private List<GameObject> landmineSpawnIndicators = new List<GameObject>();

    private bool clearLandmines = false; // true if minefield is supposed to be cleared, false if it isn't 
    
    public WaveManager waveManager; // the wave manager, calculates landmine count by doing (wave x 5) + additional landmines
    public float additionalLandmines; // extra landmines from movement thingy

    // make the landmine indicators
    private void createLandmineSpawns() {

        // make bombLimit landmines/indicators
        if (landmineSpawns.Count < bombLimit) {

            // get a random point on the floor, and store that position in a list
            transform.position = new Vector3(Random.Range(-floorX / 2, floorX / 2), 2, Random.Range(-floorZ / 2, floorZ / 2));
            landmineSpawns.Add(transform.position);

            // make a landmine indicator at that position, make it a little crooked and place it on said point
            GameObject newIndicator = Instantiate(landmineIndicator, transform.position, Quaternion.identity);
            newIndicator.transform.rotation = Quaternion.Euler(-90 + Random.Range(-15, 15), Random.Range(-180, 180), Random.Range(-180, 180));
            landmineSpawnIndicators.Add(newIndicator);

        }
    }
    
    // Start is called before the first frame update

    void Start() {

    }

    // changes value of canSpawnBombs
    // if false, 
    public void toggleSpawningBombs(bool status) {
        clearLandmines = status;

    }

    // destroy all landmines on field that aren't exploding 
    public void clearBombfield() {
        for (int i = 0; i < landmines.Count; i++) {
            if (landmines[i] != null) {
                landmines[i].GetComponent<ProjectileBehavior>().diffuse();
            }
        }
    }

    // Update is called once per frame
    void Update() {
        bombLimit = waveManager.wave * 5 + additionalLandmines;

            // if clearLandmines, clear the minefield
            if (clearLandmines) {
                createLandmineSpawns();
                clearBombfield();

            // if not, spawn a bomb at every position stored
            } else {

                // when the list that stores landmine spawnpoints isn't empty, go through it and add a landmine at the point stored at landmineSpawns[0]
                // after you spawned this landmine, get rid of landmineSpawns[0]

                // VERY IMPORTANT!!!!!! CHANGE THIS TO LANDMINE SPAWNS
                if (landmineSpawnIndicators.Count > 0) {
                    landmineSpawns.Clear();

                    // make a new landmine and add it to a list of landmines
                    GameObject newLandmine = Instantiate(landmine, landmineSpawnIndicators[0].transform.position, Quaternion.identity);
                    newLandmine.GetComponent<ProjectileBehavior>().dangerous = true;
                    landmines.Add(newLandmine);

                    // destroy landmine indicator at the spot of the new landmine
                    Destroy(landmineSpawnIndicators[0]);
                    landmineSpawnIndicators.Remove(landmineSpawnIndicators[0]);

                }

            }

        
        
    }

}
