using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PerkManager : MonoBehaviour { 
    public ModifierList modifierList;
    GameObject selectedEnemy;
    public GameObject enemy;
    public List<GameObject> perkSpawns = new List<GameObject>();
    GameObject newEnemy;

    public TimeManager timeManager;
    public List<Modifier> modifiers = new List<Modifier>();
    List<GameObject> options = new List<GameObject>();
    public WaveManager waves;

    public bool selectedPerk;
    public bool perkSelecting;

    public Material[] optionSprites = new Material[3];
    private List<GameObject> perkOptions = new List<GameObject>();

    public PlayerDamage health;
    public PlayerMovement movement;
    public FireWeapon leftDamage;
    public FireWeapon rightDamage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        

    }

    public void PerkSelected(Enemy selectedOption) {
        if (!selectedPerk) {
            Debug.Log("perk selected");
            selectedPerk = true;

            foreach (GameObject option in perkOptions) {
                if (option.GetComponent<Enemy>().getIndex() != selectedOption.GetComponent<Enemy>().getIndex()) {
                    option.GetComponent<EnemyHealth>().isPerkOption = false;
                    option.GetComponent<EnemyHealth>().takeDamage(10000000, 10000000000000);
                }
            }

            perkOptions.Clear();

            timeManager.UnpauseGame();
        }
    }

    public bool PlayerSelectedPerk() {
        return selectedPerk;

    }
    public void SpawnPerkOption(int variant) {
        if (!perkSelecting) { 
            selectedPerk = false;
            newEnemy = Instantiate(enemy, perkSpawns[variant].transform.position, Quaternion.identity);
            newEnemy.GetComponent<EnemyHealth>().changeSprite(optionSprites[variant]);
            newEnemy.GetComponent<Enemy>().setIndex(variant);
            perkOptions.Add(newEnemy);

            Debug.Log(perkOptions.Count);
        }
    }
}
