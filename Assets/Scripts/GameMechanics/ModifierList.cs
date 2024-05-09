using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModifierList : MonoBehaviour {

    public List<Modifier> modifiers = new List<Modifier>();

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void addModifier(Modifier m) {
        modifiers.Add(m);

    }

    public Modifier getModifier(float index) {
        return modifiers[(int)index];

    }

    public int getSize() {
        return modifiers.Count;

    }

}
