using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkManager : MonoBehaviour { 
    public ModifierList modifierList;
    GameObject selectedEnemy;

    public List<Modifier> modifiers = new List<Modifier>();
    List<GameObject> options = new List<GameObject>();

    private bool selectedOption;

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

        enemyStuff.setSpeed(0);
        enemyStuff.setDamage(0);
        enemyStuff.setEnemySprite(randomModifier.modifierImage);


    }

    public void PerkSelected(GameObject option) {
        Enemy enemyPerks = option.GetComponent<Enemy>();
        Debug.Log("perk selected");

    }
}
