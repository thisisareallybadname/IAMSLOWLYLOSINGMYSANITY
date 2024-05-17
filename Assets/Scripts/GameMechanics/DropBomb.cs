using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class LandmineSetter : MonoBehaviour
{

    public WaveManager manager;
    private float bombCounter;
    public float bombLimit;

    private bool canDropBombs;
    private bool spawnIndicators;

    public float floorX;
    public float floorZ;

    public GameObject landmine;
    public GameObject landmineIndicator;

    private float deleteTimer;

    private bool debounce = true;
    public bool canSpawnBombs;

    private List<Vector3> landmineSpawns = new List<Vector3>();
    private List<GameObject> landmines = new List<GameObject>();
    private List<GameObject> landmineSpawnIndicators = new List<GameObject>();

    private bool clearLandmines = false;

    private bool bombSpawnDebounce = true;
    public WaveManager waveManager;

    private void createLandmineSpawns() {
        if (landmineSpawns.Count < bombLimit) {
            transform.position = new Vector3(Random.Range(-floorX / 2, floorX / 2), 2, Random.Range(-floorZ / 2, floorZ / 2));
            landmineSpawns.Add(transform.position);
            
            GameObject newIndicator = Instantiate(landmineIndicator, transform.position, Quaternion.identity);
            landmineSpawnIndicators.Add(newIndicator);

        }
    }

    private void spawnBombs() {
        foreach (Vector3 landminePosition in landmineSpawns) {
            GameObject newLandmine = Instantiate(landmine, landminePosition, Quaternion.identity);
            newLandmine.GetComponent<ProjectileBehavior>().dangerous = true;

            landmines.Add(newLandmine);
        }
    }

    // Start is called before the first frame update

    void Start()
    {

    }

    // Update is called once per frame
    void Update() {
        bombLimit = manager.wave * 5;
            if (canSpawnBombs) {
                createLandmineSpawns();
                for (int i = 0; i < landmines.Count; i++) {
                    if (landmines[i] != null) {
                    landmines[i].GetComponent<ProjectileBehavior>().diffuse();
                    }
                }
                
            }
            else
            {
                if (landmineSpawnIndicators.Count > 0)
                {
                    landmineSpawns.Clear();
                    GameObject newLandmine = Instantiate(landmine, landmineSpawnIndicators[0].transform.position, Quaternion.identity);
                    newLandmine.GetComponent<ProjectileBehavior>().dangerous = true;
                    landmines.Add(newLandmine);

                    Destroy(landmineSpawnIndicators[0]);
                    landmineSpawnIndicators.Remove(landmineSpawnIndicators[0]);

                }

            }

        
        
    }

}