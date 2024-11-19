using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Smily : MonoBehaviour
{
    public Image image;
    void Start()
    {
        image.sprite = SpriteLoaderMaster.sprite_smilyNormal;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !Minefield.game_death && !Minefield.game_won)
        {
            image.sprite = SpriteLoaderMaster.sprite_smilyScared;
        }
        if(Input.GetMouseButtonUp(0) && !Minefield.game_death && !Minefield.game_won)
        {
            image.sprite = SpriteLoaderMaster.sprite_smilyNormal;
        }
        if(Minefield.game_death)
        {
            image.sprite = SpriteLoaderMaster.sprite_smilyDead;
        }
        if(Minefield.game_won)
        {
            image.sprite =SpriteLoaderMaster.sprite_smilyHappy;
        }
        
    }
}
