using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PerkSelectionManager : MonoBehaviour {

    public GameObject player;
    public GameObject enemy;

    public GameObject[] spawns;
    public GameObject[] UITextures;

    public Modifier[] modifiers;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowUI() {


    }

    private void SelectRandomPerks(GameObject option) {
        option.transform.GetChild(0).GetComponent<Image>().color = Color.white;

        Modifier currentModifier = modifiers[Random.Range(0, modifiers.Length)];

        TMP_Text optionDescription = option.transform.GetChild(1).GetComponent<TMP_Text>();
        TMP_Text optionTitle = option.transform.GetChild(3).GetComponent<TMP_Text>();
        Image buffImage = option.transform.GetChild(2).GetComponent<Image>();

        optionDescription.text = currentModifier.modifierDesc;
        optionTitle.text = currentModifier.modifierName;
        buffImage.sprite = currentModifier.modifierImage;


    }
}
