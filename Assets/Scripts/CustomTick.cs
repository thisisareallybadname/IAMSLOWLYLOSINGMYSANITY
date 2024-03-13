using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CustomTick : MonoBehaviour
{
    float tick = 0;
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update()
    {
        tick += 1 * Time.deltaTime;
        Debug.Log(tick);
    }

    public float getTick() {
        return tick;
    }
}
