using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    float time;

    public TextMeshProUGUI text;

    // Update is called once per frame
    void Update()
    {
        if(!Minefield.game_stopped && !Minefield.game_death && !Minefield.game_won)
        {
            time += Time.deltaTime;

            int minutes = Mathf.FloorToInt(time/60);

            int seconds = Mathf.FloorToInt(time - minutes);

            text.text = minutes+":"+seconds;

            
        }
    }
}
