using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spritetest : MonoBehaviour
{
    public Texture2D texture;
    private Sprite sprite;
    public UnityEngine.UI.Image image;

    // Start is called before the first frame update
    void Start()
    {
        sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    }

    // Update is called once per frame
    void Update()
    {
        //image.sprite = sprite;
    }
}
