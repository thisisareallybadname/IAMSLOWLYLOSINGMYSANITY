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
    public float amountOfLandmines;


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
    
    public WaveManager waveManager; // the wave manager, calculates landmine count by doing (wave x 5) + additional landmines
    public float additionalLandmines; // extra landmines from movement thingy

    // make the landmine indicators
    public void createLandmineSpawns() {
        amountOfLandmines = waveManager.getWave() * 5 + additionalLandmines;


        // make bombLimit landmines/indicators
         for (int i = 0; i < amountOfLandmines; i++) {
            // go to a random point on the floor, and store that position in a list
            transform.position = new Vector3
            (Random.Range(-floorX / 2, floorX / 2), 2, Random.Range(-floorZ / 2, floorZ / 2));
            landmineSpawns.Add(transform.position);

            // make a landmine indicator at that position, make it a little crooked and place it on said point
            GameObject newIndicator = Instantiate(landmineIndicator, transform.position, Quaternion.identity);
            Quaternion signRotation = 
            Quaternion.Euler(-90 + Random.Range(-15, 15), Random.Range(-180, 180), Random.Range(-180, 180));

            newIndicator.transform.rotation = signRotation;
            landmineSpawnIndicators.Add(newIndicator);

        }
    }

    public void resetMinefield() {
        additionalLandmines = 0;
        foreach (GameObject indicator in landmineSpawnIndicators){
            Destroy(indicator);

        }

        foreach (GameObject landmine in landmines) {
            Destroy(landmine);

        }

    }


    public void placeLandmines() {
        for (int i = 0; i < amountOfLandmines; i++) {
            GameObject newLandmine = Instantiate(landmine, landmineSpawns[0], Quaternion.identity);
            newLandmine.GetComponent<ProjectileBehavior>().enabled = true;
            landmines.Add(newLandmine);

            // destroy landmine indicator at the spot of the new landmine
            Destroy(landmineSpawnIndicators[0]);
            landmineSpawnIndicators.Remove(landmineSpawnIndicators[0]);
            landmineSpawns.Remove(landmineSpawns[0]);


        }

    }
    
    // destroy all landmines on field that aren't exploding 
    public void clearBombfield() {
        for (int i = 0; i < landmines.Count; i++) {
            if (landmines[i] != null) {
                landmines[i].GetComponent<ProjectileBehavior>().diffuse();
            }
        }
    }

}
