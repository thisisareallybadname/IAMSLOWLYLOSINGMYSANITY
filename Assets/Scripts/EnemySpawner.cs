using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject minos;
    public GameObject[] spawnpoints;

    private bool debounce;
    public float cooldown;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 spawnPos = spawnpoints[Random.Range(0, spawnpoints.Length)].transform.position;

        if (!debounce) { 
            StartCoroutine(spawnEnemy(minos, spawnPos));
        }
    }

    IEnumerator spawnEnemy(GameObject minos, Vector3 spawnPos)
    {
        
        GameObject newMinos = Instantiate(minos, spawnPos, Quaternion.identity, transform);

        debounce = true;
        yield return new WaitForSeconds(cooldown);
        debounce = false;
        
    }
}
