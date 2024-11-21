using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameUI : MonoBehaviour
{
    public TMP_InputField inputField_sizeX;
    public TMP_InputField inputField_sizeY;
    public TMP_InputField inputField_mineCount;
    void Start()
    {
        inputField_sizeX.text = Minefield.field_sizeX.ToString();
        inputField_sizeY.text = Minefield.field_sizeY.ToString();
        inputField_mineCount.text = Minefield.mine_count.ToString();

    }

    public void OnValueChange_X()
    {
        int newSizeX = int.Parse(inputField_sizeX.text);

        if(newSizeX != 1)
        {
            Minefield.field_sizeX = newSizeX;
        }
        Debug.Log(newSizeX);
    }
    
    public void OnValueChange_Y()
    {
        int newSizeY = int.Parse(inputField_sizeY.text);
        if(newSizeY != 1)
        {
            Minefield.field_sizeY = newSizeY;
        }
        Debug.Log(newSizeY);
    }
    
    public void OnValueChange_Mine()
    {
        int newMineCount = int.Parse(inputField_mineCount.text);

        int maxMines = (Minefield.field_sizeX * Minefield.field_sizeY)-1;
        if(newMineCount < maxMines)
        {
            Minefield.mine_count = newMineCount;
        }
        else Minefield.mine_count = maxMines;
        
        Debug.Log(newMineCount);
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene(0);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
