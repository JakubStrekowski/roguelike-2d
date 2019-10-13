using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    public int PosX
    {
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
            if (value > 10)
            {
                value = 10;
            }
            else
            {
                if (value < 0)
                {
                    value = 0;
                }
            }
            if (OnHealthChangeHeal != null)
            {
                if (_healthPoints < value)
                {
                    OnHealthChangeHeal();
                }
            }
            if (OnHealthChangeLose != null)
            {
                if (_healthPoints > value)
                {
                    OnHealthChangeLose();
                }
            }
            _healthPoints = value;
            OnHealthChange?.Invoke();
        }
    }
    public delegate void OnHeatlhChangeDelegate();
    public event OnHeatlhChangeDelegate OnHealthChange;

    public delegate void OnHeatlhHealDelegate();
    public event OnHeatlhHealDelegate OnHealthChangeHeal;

    public delegate void OnHeatlhLoseDelegate();
    public event OnHeatlhLoseDelegate OnHealthChangeLose;

    private bool _isDead = false;
    public bool IsDead
    {
        get => _isDead;
        set
        {
            _isDead = value;
            OnIsDeathChange?.Invoke();
        }
    }

    public delegate void OnDeathChangeDelegate();
    public event OnDeathChangeDelegate OnIsDeathChange;

    public delegate void OnItemChangeDelegate();
    public event OnItemChangeDelegate OnItemChange;

    public int viewRadius;

    private void Awake()
    {
        equipment = new Item[6];
        characterEffect = GetComponentInChildren<CharacterEffectPlayer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadDataValues();
        OnItemChange?.Invoke();
        animator = GetComponentInChildren<Animator>();
        RefreshVision();
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
                playerStandardInteraction = (IPlayerStandardInteraction)currentMap.charactersMap[posY + 1][posX].GetComponent(typeof(IPlayerStandardInteraction));
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
                animator.Play("HeroMoveUp");
                posY = posY + 1;
                break;
            case Directions.down:
                currentMap.ResetVisibility(this);
                currentMap.SwitchElements(posX, posY, posX, posY - 1);
                animator.Play("HeroMoveDown");
                posY = posY - 1;
                break;
            case Directions.right:
                currentMap.ResetVisibility(this);
                currentMap.SwitchElements(posX, posY, posX + 1, posY);
                animator.Play("HeroMoveRight");
                posX = posX + 1;
                break;
            case Directions.left:
                currentMap.ResetVisibility(this);
                currentMap.SwitchElements(posX, posY, posX - 1, posY);
                animator.Play("HeroMoveLeft");
                posX = posX - 1;
                break;
        }
        OnCharacterMoved();
    }

    private void CheckIfTheresItemOrSpecial()
    {
        if (!(currentMap.itemsMap[posY][posX] is null))
        {
            currentMap.itemsMap[posY][posX].GetComponent<Item>().OnPlayerInteract(this);
            OnItemChange?.Invoke();
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
        pdm.playerHealth = HealthPoints;
        pdm.attackValue = AttackValue;
        pdm.vision = viewRadius;

        pdm.equipment = equipment;

        foreach(Item item in equipment)
        {
            if(!(item is null))
            {
                DontDestroyOnLoad(item.gameObject);
            }
        }
    }

    

    public void UseItem(int equipmentID)
    {
        if(!( equipment[equipmentID-1] is null))
        {
            equipment[equipmentID - 1].UseItem(this);
            if(equipment[equipmentID - 1] is Consumable)
            {
                Consumable cons = equipment[equipmentID - 1].GetComponent<Consumable>();
                if (cons.numberOfUses == 0)
                {
                    equipment[equipmentID - 1] = null;
                    Destroy(cons.gameObject);
                }
            }
            OnItemChange?.Invoke();
        }
    }


}
