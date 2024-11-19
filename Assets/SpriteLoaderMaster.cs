using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Tilemaps;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System;

public class SpriteLoaderMaster : MonoBehaviour
{
    public static int loadedSpriteSet = 0;
    public static string [] directories;

    public TMP_Dropdown dropdown;

    public Tile tile_empty;
    public Tile tile_cover;
    public Tile tile_one;
    public Tile tile_two;
    public Tile tile_three;
    public Tile tile_four;
    public Tile tile_five;
    public Tile tile_six;
    public Tile tile_seven;
    public Tile tile_eight;
    public Tile tile_bomb;
    public Tile tile_bombFound;
    public Tile tile_flag;

    public static Sprite sprite_smilyNormal;
    public static Sprite sprite_smilyScared;
    public static Sprite sprite_smilyHappy;
    public static Sprite sprite_smilyDead;

    public static Sprite sprite_pointer;
    

    public static SpriteConfig spritesConfig;

    public Material backgroundMaterial;
    public Image smily_break;

    void Start()
    {
        loadDirectories();
        setTiles();
        loadConfig();
    }
    void loadDirectories()
    {
        string streamingAssetsPath = Application.streamingAssetsPath;
        directories = System.IO.Directory.GetDirectories(streamingAssetsPath);

        List<string> names = new List<string>();

        foreach(string path in directories)
        {
            string newName = path.Replace(streamingAssetsPath + @"\","");

            names.Add(newName);
        }

        dropdown.ClearOptions();
        dropdown.AddOptions(names);
    }
    void setTiles()
    {
        //TILES
        tile_cover.sprite       = loadSprite("tile_cover.png");
        tile_empty.sprite       = loadSprite("tile_empty.png");
        tile_one.sprite         = loadSprite("tile_one.png");
        tile_two.sprite         = loadSprite("tile_two.png");
        tile_three.sprite       = loadSprite("tile_three.png");
        tile_four.sprite        = loadSprite("tile_four.png");
        tile_five.sprite        = loadSprite("tile_five.png");
        tile_six.sprite         = loadSprite("tile_six.png");
        tile_seven.sprite       = loadSprite("tile_seven.png");
        tile_eight.sprite       = loadSprite("tile_eight.png");
        tile_bomb.sprite        = loadSprite("tile_bomb.png");
        tile_bombFound.sprite   = loadSprite("tile_bombFound.png");
        tile_flag.sprite        = loadSprite("tile_flag.png");

        //Smilies

        sprite_smilyNormal = loadSprite("smilyNormal.png");;
        sprite_smilyScared = loadSprite("smilyScared.png");
        sprite_smilyHappy = loadSprite("smilyHappy.png");
        sprite_smilyDead = loadSprite("smilyDead.png");

        //Pointer
        sprite_pointer = loadSprite("pointer.png");


        backgroundMaterial.SetTexture("_FrontTex",loadSprite("background.png").texture);

        smily_break.sprite = loadSprite("smilyBreak.png");

    }
    public void loadConfig()
    {
        string filePath = Path.Combine(directories[loadedSpriteSet], "config.txt");
        StreamReader reader = new StreamReader(filePath);

        spritesConfig = JsonUtility.FromJson<SpriteConfig>(reader.ReadToEnd());
    }

    public static Sprite loadSprite(string spriteName)
    {
        string filePath = Path.Combine(directories[loadedSpriteSet], spriteName);
        byte[] fileData = File.ReadAllBytes(filePath);

        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData);

        texture.filterMode = FilterMode.Point;

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f),texture.width);


        return sprite;
    }
    public void changeSprites()
    {

        loadedSpriteSet = dropdown.value;

        setTiles();
    }
}

public class SpriteConfig 
{
    public Vector2 backgroundSize = new Vector2(1,1);
    public float backgroundSpeed = 0.1f;
    public float backgroundZoomSpeed = 0.5f;
}
