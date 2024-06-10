using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PerkManager : MonoBehaviour { 

    // perk enemy stuff
    [SerializeField] GameObject perkEnemy;
    [SerializeField] List<GameObject> perkSpawns = new List<GameObject>();

    [SerializeField] TimeManager timeManager;
    List<GameObject> options = new List<GameObject>();

    [SerializeField] bool selectedPerk = false;
    [SerializeField] bool perkSelecting;

    [SerializeField] Material[] optionSprites = new Material[3];
    private List<GameObject> perkOptions = new List<GameObject>();
    
    // important player fields
    [SerializeField] PlayerDamage health;
    [SerializeField] PlayerMovement movement;
    [SerializeField] FireWeapon leftDamage;
    [SerializeField] FireWeapon rightDamage;

    [SerializeField] GameObject newProjectile;

    // modified to change the stats
    [SerializeField] GameObject originalEnemy;
    [SerializeField] LandmineSetter minefield;

    // called by perk enemy when they get hit by raycast
    // sets perk stuff
    public void PerkSelected(int variant) {
        if (!selectedPerk) {
            selectedPerk = true;

            EnemyMovement enemyMovement = originalEnemy.GetComponent<EnemyMovement>();

            if (variant == 0) { // damage
                leftDamage.SetWeaponStats(1.05f, 0.9f, "multi");
                rightDamage.SetWeaponStats(1.1f, 0.95f, "multi");

                enemyMovement.ModifyProjectileVariables(0.85f, 0.85f, 1.25f, "multi");

            } else if (variant == 1) { // health
                health.maxHealth += 1;
                enemyMovement.setWalkspeed(1.25f, "multi");

            } else if (variant == 2) { // speed
                movement.setMovementStats(0.25f, 0.25f, "add");
                minefield.additionalLandmines += 5;

            }

            // destroy the other perk enemies to show which ones are which
            foreach (GameObject option in perkOptions) {
                float index = option.GetComponent<EnemyHealth>().getIndex();
                if (index != variant) {
                    Destroy(option.gameObject);
                }
            }

            timeManager.UnpauseGame();

            perkOptions.Clear();
            
        }
    }

    public void setSelectedPerkStatus(bool status) {
        selectedPerk = status;

    }

    public bool PlayerSelectedPerk() {
        return selectedPerk;

    }

    // spawn perk option
    public void SpawnPerkOption(int variant) {
        if (!perkSelecting) { 
            selectedPerk = false;

            // physically create a new perk option
            GameObject newEnemy = Instantiate(perkEnemy, perkSpawns[variant].transform.position, Quaternion.identity);
            newEnemy.GetComponent<EnemyHealth>().changeSprite(optionSprites[variant]);
            newEnemy.GetComponent<EnemyHealth>().setIndex(variant);

            perkOptions.Add(newEnemy);

        }
    }
}
