using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public Tile[][] tileMap;
    public Character[][] charactersMap;
    public Item[][] itemsMap;
    int mapRowLimit;
    int mapColumnLimit;

    //public EnemyIterator enemies;

    public Map(int[][] intMap, int rowAmmount, int ColumnAmmount)
    {
        mapRowLimit = rowAmmount;
        mapColumnLimit = ColumnAmmount;
        this.tileMap = new Tile[mapRowLimit][];
        this.charactersMap = new Character[mapRowLimit][];
        this.itemsMap = new Item[mapRowLimit][];
        int rowCounter = 0;
        foreach (int[] intRow in intMap)
        {
            this.tileMap[rowCounter] = new Tile[mapColumnLimit];
            this.charactersMap[rowCounter] = new Character[mapColumnLimit];
            this.itemsMap[rowCounter] = new Item[mapColumnLimit];
            int columnCounter = 0;
            foreach (int integer in intRow)
            {
                if(integer == 0 || integer == 1 || integer == 4)
                {
                    this.tileMap[rowCounter][columnCounter] =
                        GameManager._instance.tileFactory.Get(integer, columnCounter, rowCounter, this).GetComponent<Tile>();
                }
                else
                {
                    this.tileMap[rowCounter][columnCounter] =
                        GameManager._instance.tileFactory.Get(0, columnCounter, rowCounter, this).GetComponent<Tile>();
                    if(integer==2|| integer == 3 || integer == 6 || integer == 8 || integer == 9)//if it's a character
                    {
                        this.charactersMap[rowCounter][columnCounter] =
                            GameManager._instance.tileFactory.Get(integer, columnCounter, rowCounter, this).GetComponent<Character>(); //creating character
                    }
                    else if(integer == 5 || integer == 7)
                    {
                        this.itemsMap[rowCounter][columnCounter] =
                            GameManager._instance.tileFactory.Get(integer, columnCounter, rowCounter, this).GetComponent<Item>();
                    }

                }
                columnCounter++;
            }
            rowCounter++;
        }
        //enemies = new EnemyIterator(tileMap);
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
                if (charactersMap[posY - 1][posX] != null) return charactersMap[posY - 1][posX].gameObject;
                return tileMap[posY - 1][posX].gameObject;
            case Character.Directions.down:
                if (charactersMap[posY + 1][posX] != null) return charactersMap[posY + 1][posX].gameObject;
                return tileMap[posY + 1][posX].gameObject;
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
                if(!tileMap[posY - 1][posX].isPassable)
                {
                    return 1;
                }
                else if(charactersMap[posY-1][posX]!=null)
                {
                    if (charactersMap[posY - 1][posX] is PlayerCharacter)
                    {
                        return 3;
                    }
                    else return 2;
                }
                return 0;
            case Character.Directions.down:
                if (!tileMap[posY + 1][posX].isPassable)
                {
                    return 1;
                }
                else if (charactersMap[posY + 1][posX] != null)
                {
                    if (charactersMap[posY + 1][posX] is PlayerCharacter)
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

}
