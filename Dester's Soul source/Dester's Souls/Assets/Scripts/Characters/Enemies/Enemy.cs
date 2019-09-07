using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Enemy : Character
{
    public int id;
    public int speed;
    public int giveGold;

    void Start()
    {
        
    }

    public abstract void MovementBehaviour();

    public bool MoveDirection(Directions direction)
    {
        int targetPositionX = posX;
        int targetPositionY = posY;
        int tileInfo = 0;
        switch (direction)
        {
            case Directions.up:
                targetPositionY = targetPositionY - 1;
                tileInfo = currentMap.IsTilePassable(posX, posY, 0);
                if (tileInfo == 0)
                {
                    currentMap.SwitchElements(posX, posY, targetPositionX, targetPositionY);
                    posY = posY - 1;
                    return true;
                }
                else
                {
                    if (tileInfo == 3)
                    {
                        PlayerCharacter enm = (PlayerCharacter)currentMap.GiveNeighbourGameObject(posX, posY, 0).GetComponent<PlayerCharacter>();
                        enm.OnTakenDamage(AttackValue);
                        //currentMap.SendLog(name + " hit you for " + attack.ToString() + " damage!");
                    }
                }
                break;
            case Directions.down:
                targetPositionY = targetPositionY + 1;
                tileInfo = currentMap.IsTilePassable(posX, posY, Directions.down);

                if (tileInfo==0)
                {
                    currentMap.SwitchElements(posX, posY, targetPositionX, targetPositionY);
                    posY = posY + 1;
                    return true;
                }
                else
                {
                    if (tileInfo==3)
                    {
                        PlayerCharacter enm = (PlayerCharacter)currentMap.GiveNeighbourGameObject(posX, posY, Directions.down).GetComponent<PlayerCharacter>();
                        enm.OnTakenDamage(AttackValue);
                        //currentMap.SendLog(name + " hit you for " + attack.ToString() + " damage!");
                    }
                }
                break;
            case Directions.right:
                targetPositionX = targetPositionX + 1;
                tileInfo = currentMap.IsTilePassable(posX, posY, Directions.right);

                if (tileInfo==0)
                {
                    currentMap.SwitchElements(posX, posY, targetPositionX, targetPositionY);
                    posX = posY + 1;
                    return true;
                }
                else
                {
                    if (tileInfo==3)
                    {
                        PlayerCharacter enm = (PlayerCharacter)currentMap.GiveNeighbourGameObject(posX, posY, Directions.right).GetComponent<PlayerCharacter>();
                        enm.OnTakenDamage(AttackValue);
                        //currentMap.SendLog(name + " hit you for " + attack.ToString() + " damage!");
                    }
                }

                break;
            case Directions.left:
                targetPositionX = targetPositionX - 1;
                tileInfo = currentMap.IsTilePassable(posX, posY, Directions.left);

                if (tileInfo==0)
                {
                    currentMap.SwitchElements(posX, posY, targetPositionX, targetPositionY);
                    posX = posX - 1;
                    return true;
                }
                else
                {
                    if (tileInfo==3)
                    {
                        PlayerCharacter enm = (PlayerCharacter)currentMap.GiveNeighbourGameObject(posX, posY, Directions.left).GetComponent<PlayerCharacter>();
                        enm.OnTakenDamage(AttackValue);
                        //currentMap.SendLog(name + " hit you for " + attack.ToString() + " damage!");
                    }
                }
                break;
        }
        return false;
    }
}
