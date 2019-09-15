using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    public int PosX {
        get
        {
            return posX;
        }
    }

    public int PosY
    {
        get
        {
            return posY;
        }
    }

    public int viewRadius;

    // Start is called before the first frame update
    void Start()
    {
        AttackValue = 1;
        HealthPoints = 5;
        viewRadius = 5;
        RefreshVision();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    public void Move(Directions direction)
    {
        int collisionType = currentMap.IsTilePassable(posX, posY, direction);
        if (collisionType == 0)
        {
            OnEmptySpaceCollision(direction);
            CheckIfTheresItem();
        }

        if (collisionType == 2)
        {
            OnCharacterOrSpecialCollision(direction);
        }

    }

    private void OnCharacterOrSpecialCollision(Directions direction)
    {
        IPlayerStandardInteraction playerStandardInteraction;
        switch (direction)
        {
            case Directions.up:
                playerStandardInteraction =(IPlayerStandardInteraction)currentMap.charactersMap[posY + 1][posX].GetComponent(typeof(IPlayerStandardInteraction));
                playerStandardInteraction.OnPlayerInteract(this);
                break;
            case Directions.down:
                playerStandardInteraction = (IPlayerStandardInteraction)currentMap.charactersMap[posY - 1][posX].GetComponent(typeof(IPlayerStandardInteraction));
                playerStandardInteraction.OnPlayerInteract(this);
                break;
            case Directions.right:
                playerStandardInteraction = (IPlayerStandardInteraction)currentMap.charactersMap[posY][posX + 1].GetComponent(typeof(IPlayerStandardInteraction));
                playerStandardInteraction.OnPlayerInteract(this);
                break;
            case Directions.left:
                playerStandardInteraction = (IPlayerStandardInteraction)currentMap.charactersMap[posY][posX - 1].GetComponent(typeof(IPlayerStandardInteraction));
                playerStandardInteraction.OnPlayerInteract(this);
                break;
        }
        
    }

    private void OnEmptySpaceCollision(Directions direction)
    {
        switch (direction)
        {
            case Directions.up:
                currentMap.ResetVisibility(this);
                currentMap.SwitchElements(posX, posY, posX, posY + 1);
                    posY = posY + 1;
                break;
            case Directions.down:
                currentMap.ResetVisibility(this);
                currentMap.SwitchElements(posX, posY, posX, posY - 1);
                posY = posY - 1;
                break;
            case Directions.right:
                currentMap.ResetVisibility(this);
                currentMap.SwitchElements(posX, posY, posX + 1, posY);
                posX = posX + 1;
                break;
            case Directions.left:
                currentMap.ResetVisibility(this);
                currentMap.SwitchElements(posX, posY, posX - 1, posY);
                posX = posX - 1;
                break;
        }
        OnCharacterMoved();
    }

    private void CheckIfTheresItem()
    {
        if(!(currentMap.itemsMap[posY][posX]is null))
        {
            currentMap.itemsMap[posY][posX].GetComponent<Item>().OnPlayerInteract(this);
        }
    }

    void RefreshVision()
    {
        currentMap.FOV(this);
    }
    
    

    public override void OnDeath()
    {
        //throw new System.NotImplementedException();
    }
}
