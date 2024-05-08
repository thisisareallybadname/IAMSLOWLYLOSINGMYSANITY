using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LandmineSetter : MonoBehaviour
{

    public WaveManager manager;
    private float bombCounter;
    public float bombLimit;

    private bool canDropBombs;
    private bool spawningLandmineIndicators;

    public float floorX;
    public float floorZ;

    public GameObject landmine;
    public GameObject landmineIndicator;

    private HashSet<GameObject> landmineIndicators = new HashSet<GameObject>();
    private HashSet<GameObject> landmines = new HashSet<GameObject>();

    // Start is called before the first frame update

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        spawningLandmineIndicators = manager.intermission;
        canDropBombs = manager.spawningEnemies;

        bombLimit = manager.wave * 5;

        if (spawningLandmineIndicators)
        {
            if (bombCounter < bombLimit)
            {
                transform.position = new Vector3(Random.Range(-floorX / 2, floorX / 2), 3, Random.Range(-floorZ / 2, floorZ / 2));
                GameObject newLandmineIndicator = Instantiate(landmineIndicator, transform.position, Quaternion.identity);
                landmineIndicators.Add(newLandmineIndicator);

                bombCounter++;

            }

        } else {

            if (canDropBombs) {
                foreach (GameObject indicator in landmineIndicators) {
                    GameObject newLandmine = Instantiate(landmine, indicator.transform.position, Quaternion.identity);
                    newLandmine.GetComponent<ProjectileBehavior>().dangerous = true;
                    landmines.Add(newLandmine);
                    //Destroy(indicator);
                    //Debug.Log(landmineIndicators.Count - landmines.Count);
                }

            } else {
                bombCounter = 0;

                if (manager.EnemiesLeft() == 0) {
                    foreach (GameObject landmine in landmines) {
                        Destroy(landmine);

                    }
                }
            }
        }
    }

}