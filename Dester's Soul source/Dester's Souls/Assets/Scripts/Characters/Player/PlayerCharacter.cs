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

    public override int HealthPoints
    {
        get => _healthPoints;
        set
        {
            _healthPoints = value;
            if (OnHealthChange != null)
                OnHealthChange();
        }
    }
    public delegate void OnHeatlhChangeDelegate();
    public event OnHeatlhChangeDelegate OnHealthChange;

    private bool _isDead = false;
    public bool IsDead
    {
        get => _isDead;
        set
        {
            _isDead = value;
            if (OnIsDeathChange != null)
                OnIsDeathChange();
        }
    }

    public delegate void OnDeathChangeDelegate();
    public event OnDeathChangeDelegate OnIsDeathChange;

    public int viewRadius;

    // Start is called before the first frame update
    void Start()
    {
        LoadDataValues();
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
            CheckIfTheresItemOrSpecial();
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

    private void CheckIfTheresItemOrSpecial()
    {
        if(!(currentMap.itemsMap[posY][posX]is null))
        {
            currentMap.itemsMap[posY][posX].GetComponent<Item>().OnPlayerInteract(this);
        }
        currentMap.tileMap[posY][posX].GetComponent<Tile>().OnPlayerInteract(this);
    }

    void RefreshVision()
    {
        currentMap.FOV(this);
    }
    
    public override void OnDeath()
    {
        IsDead = true;
        //throw new System.NotImplementedException();
    }

    public void LoadDataValues()
    {
        PlayerDataManager pdm = GameManager._instance.gameObject.GetComponent<PlayerDataManager>();
        HealthPoints = pdm.playerHealth;
        AttackValue = pdm.attackValue;
        viewRadius = pdm.vision;

        equipment = pdm.equipment;
    }

    public void SetDataValues()
    {
        PlayerDataManager pdm = GameManager._instance.gameObject.GetComponent<PlayerDataManager>();
        pdm.playerHealth=HealthPoints;
        pdm.attackValue=AttackValue;
        pdm.vision = viewRadius;

        pdm.equipment=equipment;
    }
}
