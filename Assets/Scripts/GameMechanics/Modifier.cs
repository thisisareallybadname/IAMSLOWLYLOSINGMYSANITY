using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Modifier {

    public float damageModifier;
    public float speedModifier;
    public float healthModifier;

    public string modifierDesc;
    public string modifierName;
    public Material modifierImage;

    public Modifier(float newDamage, float newSpeed, float newHealth, Material newModifierImage) {
        damageModifier = newDamage;
        speedModifier = newSpeed;
        healthModifier = newHealth;
        modifierImage = newModifierImage;
    }
}
