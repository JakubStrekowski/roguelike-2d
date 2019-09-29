﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    public Tile[][] tileMap;
    public Character[][] charactersMap;
    public Item[][] itemsMap;
    int mapRowLimit;
    int mapColumnLimit;

    //public EnemyIterator enemies;

    public Map(DungeonStruct dungeonStructure, int rowAmmount, int ColumnAmmount, TurnManager turnManager)
    {
        mapRowLimit = rowAmmount;
        mapColumnLimit = ColumnAmmount;
        this.tileMap = new Tile[mapRowLimit][];
        this.charactersMap = new Character[mapRowLimit][];
        this.itemsMap = new Item[mapRowLimit][];
        int rowCounter = 0;

        foreach (int[] intRow in dungeonStructure.dungeon)
        {
            this.tileMap[rowCounter] = new Tile[mapColumnLimit];
            int columnCounter = 0;
            foreach (int integer in intRow)
            {
                 this.tileMap[rowCounter][columnCounter] =
                    GameManager._instance.tileFactory.GetTile(integer, columnCounter, rowCounter, this).GetComponent<Tile>();
              
                columnCounter++;
            }
            rowCounter++;
        }
        rowCounter = 0;
        foreach (int[] intRow in dungeonStructure.characterMap)
        {
            charactersMap[rowCounter] = new Character[mapColumnLimit];
            int columnCounter = 0;
            foreach (int integer in intRow)
            {
                if (integer != 0) //zero is reserved for no characters
                {
                    charactersMap[rowCounter][columnCounter] =
                   GameManager._instance.tileFactory.GetCharacter(integer, columnCounter, rowCounter, this).GetComponent<Character>();
                    if (integer == 1)
                    {
                        turnManager.hero = charactersMap[rowCounter][columnCounter] as PlayerCharacter;
                        turnManager.hero.OnHealthChange += turnManager.UpdateHeroHealthGUI;
                        turnManager.hero.OnIsDeathChange += turnManager.UpdateHeroDeathGUI;
                    }
                    else
                    {
                        turnManager.enemyList.Add(charactersMap[rowCounter][columnCounter] as Enemy);
                    }
                }
                columnCounter++;
            }
            rowCounter++;
        }
        rowCounter = 0;
        foreach (int[] intRow in dungeonStructure.itemMap)
        {
            this.itemsMap[rowCounter] = new Item[mapColumnLimit];
            int columnCounter = 0;
            foreach (int integer in intRow)
            {
                if (integer != 0)//zero reserved for no items
                {
                    this.itemsMap[rowCounter][columnCounter] =
                       GameManager._instance.tileFactory.GetItem(integer, columnCounter, rowCounter, this).GetComponent<Item>();
                }
                columnCounter++;
            }
            rowCounter++;
        }
    }

    public Tile GiveNeighbourTile(int posX, int posY, Character.Directions direction) 
    {
        switch (direction)
        {
            case Character.Directions.up:return tileMap[posY - 1][posX];
            case Character.Directions.down: return tileMap[posY + 1][posX];
            case Character.Directions.right: return tileMap[posY][posX + 1];
            case Character.Directions.left: return tileMap[posY][posX - 1];
            default: return tileMap[posY][posX];
        }
    }

    public void SwitchTiles(int sourceX, int sourceY, int targetX, int targetY)
    {
        Tile temporary = tileMap[targetY][targetX];
        tileMap[targetY][targetX] = tileMap[sourceY][sourceX];
        tileMap[sourceY][sourceX] = temporary;
    }

    public void SwitchElements(int sourceX, int sourceY, int targetX, int targetY)
    {
        Character temporary = charactersMap[targetY][targetX];
        charactersMap[targetY][targetX] = charactersMap[sourceY][sourceX];
        charactersMap[sourceY][sourceX] = temporary;
    }

    public GameObject GiveNeighbourGameObject(int posX, int posY, Character.Directions direction)
    {
        switch (direction)
        {
            case Character.Directions.up:
                if (charactersMap[posY + 1][posX] != null) return charactersMap[posY + 1][posX].gameObject;
                return tileMap[posY + 1][posX].gameObject;
            case Character.Directions.down:
                if (charactersMap[posY - 1][posX] != null) return charactersMap[posY - 1][posX].gameObject;
                return tileMap[posY - 1][posX].gameObject;
            case Character.Directions.right:
                if (charactersMap[posY][posX + 1] != null) return charactersMap[posY][posX + 1].gameObject;
                return tileMap[posY][posX + 1].gameObject;
            case Character.Directions.left:
                if (charactersMap[posY][posX - 1] != null) return charactersMap[posY][posX - 1].gameObject;
                return tileMap[posY][posX - 1].gameObject;
            default: return tileMap[posY][posX].gameObject;
        }
    }

    /*
     * returns:
     * 0-passable
     * 1-not passable-wall
     * 2-not passable-enemy/special item
     * 3-not passable-hero
     * */
    public int IsTilePassable(int posX, int posY, Character.Directions direction) 
    {
        switch (direction)
        {
            case Character.Directions.up:
                if(!tileMap[posY + 1][posX].isPassable)
                {
                    return 1;
                }
                else if(charactersMap[posY+1][posX]!=null)
                {
                    if (charactersMap[posY + 1][posX] is PlayerCharacter)
                    {
                        return 3;
                    }
                    else return 2;
                }
                return 0;
            case Character.Directions.down:
                if (!tileMap[posY - 1][posX].isPassable)
                {
                    return 1;
                }
                else if (charactersMap[posY - 1][posX] != null)
                {
                    if (charactersMap[posY - 1][posX] is PlayerCharacter)
                    {
                        return 3;
                    }
                    else return 2;
                }
                return 0;
            case Character.Directions.right:
                if (!tileMap[posY][posX+1].isPassable)
                {
                    return 1;
                }
                else if (charactersMap[posY][posX+1] != null)
                {
                    if (charactersMap[posY][posX+1] is PlayerCharacter)
                    {
                        return 3;
                    }
                    else return 2;
                }
                return 0;
            case Character.Directions.left:
                if (!tileMap[posY][posX - 1].isPassable)
                {
                    return 1;
                }
                else if (charactersMap[posY][posX - 1] != null)
                {
                    if (charactersMap[posY][posX - 1] is PlayerCharacter)
                    {
                        return 3;
                    }
                    else return 2;
                }
                return 0;
            default: return 1;
        }
    }

    public void FOV(PlayerCharacter player)
    {
        float x, y;
        int i;
        ResetVisibility(player);
        for (i = 0; i < 360; i++)
        {
            x = Mathf.Cos((float)i * 0.01745f);
            y = Mathf.Sin((float)i * 0.01745f);
            DoFov(x, y,player);
        };
    }

    void DoFov(float x, float y, PlayerCharacter player)
    {
        int i;
        float ox, oy;
        ox = (float)player.PosX + 0.5f;
        oy = (float)player.PosY + 0.5f;
        IVisibility visibility;
        for (i = 0; i < player.viewRadius; i++)
        {
            visibility=(IVisibility)tileMap[(int)oy][(int)ox].GetComponent(typeof(IVisibility));//Set the tile to visible.
            visibility.TurnVisible();
            if (charactersMap[(int)oy][(int)ox] != null)
            {
                if ((int)oy != player.PosY || (int)ox != player.PosX)
                {
                    visibility = (IVisibility)charactersMap[(int)oy][(int)ox].GetComponent(typeof(IVisibility));//Set the tile to visible.
                    visibility.TurnVisible();
                }
            }
            
            if (itemsMap[(int)oy][(int)ox]!=null)
            {
                
                visibility = (IVisibility)itemsMap[(int)oy][(int)ox].GetComponent(typeof(IVisibility));//Set the tile to visible.
                visibility.TurnVisible();
            }
            if (tileMap[(int)oy][(int)ox].isPassable==false)
                return;
            ox += x;
            oy += y;
        }
    }

    public void ResetVisibility(PlayerCharacter player)
    {
        IVisibility visibility;

        int beginX =player.PosX-(player.viewRadius+1)>0? player.PosX - player.viewRadius:0;
        int beginY=player.PosY - (player.viewRadius + 1) > 0 ? player.PosY - player.viewRadius : 0;

        int endX = player.PosX + (player.viewRadius + 1) < mapColumnLimit ? player.PosX + player.viewRadius : mapColumnLimit;
        int endY = player.PosY + (player.viewRadius + 1) < mapRowLimit ? player.PosY + player.viewRadius : mapRowLimit;

        for (int i = beginY; i < endY; i++)
        {
            for(int j = beginX; j < endX; j++)
            {
                
                visibility = (IVisibility)tileMap[i][j].GetComponent(typeof(IVisibility));//Set the tile to visible.
                visibility.TurnInvisible();
                if (charactersMap[i][j]!=null)
                {
                    if (i != player.PosY || j != player.PosX)
                    {
                        visibility = (IVisibility)charactersMap[i][j].GetComponent(typeof(IVisibility));//Set the tile to visible.
                        visibility.TurnInvisible();
                    }
                }
                if (itemsMap[i][j]!=null)
                {
                    visibility = (IVisibility)itemsMap[i][j].GetComponent(typeof(IVisibility));//Set the tile to visible.
                    visibility.TurnInvisible();
                }
                
            }
        }
    }

}
