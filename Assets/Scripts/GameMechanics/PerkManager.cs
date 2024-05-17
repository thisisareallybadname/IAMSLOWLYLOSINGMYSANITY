using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkManager : MonoBehaviour { 
    public ModifierList modifierList;
    GameObject selectedEnemy;
    public GameObject enemy;
    public GameObject perkSpawn;
    GameObject newEnemy;

    public List<Modifier> modifiers = new List<Modifier>();
    List<GameObject> options = new List<GameObject>();
    public WaveManager waves;

    public bool selectedPerk;
    public bool perkSelecting;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        

    }

    void ShowOptions() {
        


    }
    void MakeOptionEffect(GameObject perkOption)
    {
        float randomIndex = Random.Range(0, modifierList.getSize() - 1);
        Modifier randomModifier = modifierList.getModifier(randomIndex);
        Enemy enemyStuff = perkOption.GetComponent<Enemy>();

        enemyStuff.setEnemySprite(randomModifier.modifierImage);


    }

    public void PerkSelected(Enemy stats) {
        Debug.Log("perk selected");
        perkSelecting = false;
        selectedPerk = true;
        waves.startGame();

    }

    public void SpawnPerkOption() {
        if (!perkSelecting) {
            perkSelecting = true;
            newEnemy = Instantiate(enemy, perkSpawn.transform.position, Quaternion.identity);
        }
    }
}
