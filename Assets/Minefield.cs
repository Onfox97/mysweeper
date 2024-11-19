
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Minefield : MonoBehaviour
{
    [Header("Settings")]
    public static int field_sizeX = 20; //velikost pole x
    public static int field_sizeY = 20; //velikost pole y

    public static int mine_count = 40;  //asi nemusím vysvětlovat
    [Header("Tiles")]
    public Tile tile_cover;
    public Tile[] tile_numbers;
    public Tile tile_flag;
    public Tile tile_mine;
    public Tile tile_mineFound;
    [Header("TileMaps")]
    public Tilemap tilemap_cover;   
    public Tilemap tilemap_field;

    [Header("GameData")]
    public List<Vector3Int> mine_positions; //uchovává pozice min
    public List<Vector3Int> empty_positions; // uchovává prázdné místa(žádné mini, žádná čísla)
    public List<Vector3Int> uncovered_positions;//uchovává odhalené místa
    public List<Vector3Int> flag_positions;

    int mine_flagged;
    public static int flag_placed; 
    public float uncover_speed = 0.2f;
    bool game_start;
    public static bool game_death = false;
    public static bool game_won = false;
    public static bool game_stopped = false;
    Vector3Int game_startPosition;
    void Start()
    {
        GenerateCover();

        mine_flagged = 0;
        flag_placed = 0;
        game_death = false;
        game_won = false;
        game_stopped = false;

    }
    void Update()
    {
        if(!game_death && !game_won && !game_stopped)
        {
            Vector3Int mousePosition = getMousePoos();
            if(Input.GetMouseButtonDown(0))
            {

            }
            if(Input.GetMouseButtonUp(0))
            {
                Uncover(mousePosition.x,mousePosition.y);
            }
            if(Input.GetMouseButtonDown(1))
            {
                PlaceFlag(mousePosition.x,mousePosition.y);
            }
        }
    }
    void PlaceFlag(int x,int y)
    {
        if(x >= 0 && x < field_sizeX)
        {
            if(y >= 0 && y < field_sizeY)
            {
                Vector3Int poos = new Vector3Int(x,y,0);

                if(!uncovered_positions.Contains(poos))
                {
                    if(flag_positions.Contains(poos))
                    {
                        RemoveFlag(x,y);
                    }
                    else
                    {   
                        flag_placed ++;
                        //pokud je vlajka na mině, přičíst počítadlo najitých min
                        if(mine_positions.Contains(poos))
                        {
                            mine_flagged ++;
                            if(mine_flagged == mine_count && mine_flagged == flag_placed)
                            {
                                AllMinesFlagged();
                            }
                        }

                        

                        flag_positions.Add(poos);
                        SetTile(x,y,tilemap_cover,tile_flag);
                    }
                }
            }
        }
    }
    void RemoveFlag(int x,int y)
    {   
        Vector3Int poos = new Vector3Int(x,y,0);

        if(mine_positions.Contains(poos))
        {
            mine_flagged --;
        }

        flag_placed --;

        flag_positions.Remove(poos);
        SetTile(x,y,tilemap_cover,tile_cover);
    }
    void AllMinesFlagged()
    {
        game_won = true;
        Debug.Log("Victory");
    }
    void Uncover(int x,int y)
    {
        if(x >= 0 && x < field_sizeX)
        {
            if(y >= 0 && y < field_sizeY)
            {
                Vector3Int position = new Vector3Int(x,y,0);
                if(game_start == false)
                {   
                    game_start = true;
                    game_startPosition = position;

                    GenerateMines();
                    GenerateNumbers();
                }
                if(!uncovered_positions.Contains(position))
                {
                    //pokud je místo prázdné
                    if(empty_positions.Contains(position)) StartCoroutine(UncoverEmpty(x,y));
                    else//pokud je na místě mina
                    if(mine_positions.Contains(position)) UncoverMine(x,y);
                    else UncoverNumber(x,y);
                }
            }
        }
    }
    IEnumerator UncoverEmpty(int x,int y)
    {
        Vector3Int position = new Vector3Int(x,y,0);
        uncovered_positions.Add(position);

        RemoveTile(x,y,tilemap_cover);

        yield return new WaitForSeconds(uncover_speed);
        //zkontroluje jestli v okolí nejsou další prázné místa nebo čísla, pokud ano tak je odkryje
        for(int i = 0; i< 8;i++)
        {
            Vector3Int checkPoos = position + checkPositions[i];
            
            //pokud na pozicí není mina nebo vlajka tak pokračovat
            if(!uncovered_positions.Contains(checkPoos) && !flag_positions.Contains(checkPoos))
            {
                if(empty_positions.Contains(checkPoos)) StartCoroutine(UncoverEmpty(checkPoos.x,checkPoos.y));
                else UncoverNumber(checkPoos.x,checkPoos.y);
            }
        }
    }
    void UncoverNumber(int x,int y)
    {
        Vector3Int position = new Vector3Int(x,y,0);

        uncovered_positions.Add(position);

        RemoveTile(x,y,tilemap_cover);
    }
    void UncoverMine(int x,int y)
    {
        Vector3Int position = new Vector3Int(x,y,0);

        uncovered_positions.Add(position);

        RemoveTile(x,y,tilemap_cover);

        SetTile(x,y,tilemap_field,tile_mineFound);

        game_death = true;

        ClearAllCover();
    }
    void ClearAllCover()
    {
        tilemap_cover.ClearAllTiles();
    }
    Vector3Int getMousePoos()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        int x = Mathf.RoundToInt(mousePosition.x);
        int y = Mathf.RoundToInt(mousePosition.y);

        return new Vector3Int(x,y,0);
    }

    void GenerateMines()
    {
        for(int i = 0; i < mine_count;i++)
        {   
            Vector3Int poos = GetRandomMinePoos();

            SetTile(poos.x,poos.y,tilemap_field,tile_mine);
            mine_positions.Add(poos);
        }
    }
    void GenerateNumbers()
    {
        for(int x = 0;x < field_sizeX;x++)
        {
            for(int y = 0;y < field_sizeY; y++)
            {   
                Vector3Int position = new Vector3Int(x,y,0);

                if(!mine_positions.Contains(position))
                {
                    //získá počet min v okolí
                    int tNumber = GetTileNumber(x, y);

                    //pokud v okolí žádné mini nejsou, pozice se přidá do sezamu prázdných míst
                    if(tNumber == 0) empty_positions.Add(position);

                    //položí tile pro dané číslo
                    SetTile(x,y,tilemap_field,tile_numbers[tNumber]);
                }
            }
        }
    }
    readonly Vector3Int[] checkPositions = new Vector3Int[8]
    {
        new Vector3Int(1,0,0),
        new Vector3Int(1,1,0),
        new Vector3Int(0,1,0),
        new Vector3Int(-1,1,0),
        new Vector3Int(-1,0,0),
        new Vector3Int(-1,-1,0),
        new Vector3Int(0,-1,0),
        new Vector3Int(1,-1,0)
    };
    int GetTileNumber(int x, int y)
    {
        int count = 0;
        for(int i = 0; i < 8; i++)
        {
            Vector3Int deltaPoos = checkPositions[i];

            Vector3Int check = new Vector3Int(x,y,0)+deltaPoos;

            if(mine_positions.Contains(check))
            {
                count++;
            }
        }
        return count;


    }
    //pozice pro detekci min

    Vector3Int GetRandomMinePoos()
    {
        
        Vector3Int minePoos = new Vector3Int();

         
        for(bool yesSir = false; yesSir == false;)
        {
            int x = Random.Range(0,field_sizeX);
            int y = Random.Range(0,field_sizeY);

            minePoos = new Vector3Int(x,y,0);


            bool foundMatch = false;
            for(int i =0;i < mine_positions.Count;i++)
            {
                Vector3Int poos = mine_positions.ToArray()[i];
                if(minePoos == poos && minePoos ==game_startPosition)
                {
                    foundMatch = true;
                    break;
                }
            }

            if(!foundMatch) yesSir = true;
        }

        return minePoos;
        
    }
    void GenerateCover()
    {
        for(int x = 0;x < field_sizeX;x++)
        {
            for(int y = 0;y < field_sizeY; y++)
            {
                SetTile(x,y,tilemap_cover,tile_cover);//popředí
            }
        }
    }

    void SetTile(int x,int y,Tilemap tilemap,Tile tile)
    {
        Vector3Int poos = new Vector3Int(x,y,0);
        tilemap.SetTile(poos,tile);
    }
    void RemoveTile(int x,int y,Tilemap tilemap)
    {
        Vector3Int poos = new Vector3Int(x,y,0);
        tilemap.SetTile(poos,null);
    }

    public void StopGame(bool state)
    {
        game_stopped = state;
    }
}
