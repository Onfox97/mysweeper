using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI text_flagCounter;
    public Transform cursor;
    SpriteRenderer ren_cursor;
    void Start()
    {
        ren_cursor = cursor.GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        text_flagCounter.text =(Minefield.mine_count-Minefield.flag_placed).ToString();

        //curzor poos
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        int x = Mathf.RoundToInt(mousePosition.x);
        int y = Mathf.RoundToInt(mousePosition.y);

        cursor.position =  new Vector3Int(x,y,0);

        ren_cursor.sprite = SpriteLoaderMaster.sprite_pointer;
        
    }

}
