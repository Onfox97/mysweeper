using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLoader : MonoBehaviour
{
    SpriteRenderer ren;
    public string spriteName;
    void Start()
    {
        ren = GetComponent<SpriteRenderer>();
        reloadSprite();
    }

    public void reloadSprite()
    {
        ren.sprite = SpriteLoaderMaster.loadSprite(spriteName);
    }
}
