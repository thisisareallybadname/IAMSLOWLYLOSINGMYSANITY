using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ui : MonoBehaviour {

    public VMEffects bobEffect;
    private Vector3 playerBob;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        playerBob = bobEffect.getBobVector();
        GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -75) + new Vector2(playerBob.x, playerBob.y) / 15;
    }
}
