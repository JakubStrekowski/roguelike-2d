﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Enemy : Character, IPlayerStandardInteraction, IVisibility
{
    public int speed;
    public int giveGold;

    public AudioClip dieSound;

    public EnemyHealthRepresentation enemyHealthRepresentation;

    public abstract void MovementBehaviour();

    protected int lastHeroPosX;
    protected int lastHeroPosY;
    protected SpriteRenderer spriteRenderer;

    public int LastHeroPosX { get => lastHeroPosX;}
    public int LastHeroPosY { get => lastHeroPosY;}
    public int memoryTurns;

    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        characterEffect = GetComponentInChildren<CharacterEffectPlayer>();
        lastHeroPosX = -1;
        lastHeroPosY = -1;
        memoryTurns = 0;
    TurnInvisible();
    }

    public override void OnTakenDamage(int damageAmmount)
    {
        HealthPoints -= damageAmmount;
        enemyHealthRepresentation.UpdateEnemyHealth(HealthPoints);
        characterEffect.PlayHurtAnimation();
        StartCoroutine("StartBleeding");
        if (HealthPoints <= 0)
        {
            AudioSource.PlayClipAtPoint(dieSound, turnManager.mainCamera.transform.position, GameManager._instance.GetComponent<GameDataManager>().SoundsVolume);
            OnDeath();
        }
        else
        {
            AudioSource.PlayClipAtPoint(gotHitSound, turnManager.mainCamera.transform.position, GameManager._instance.GetComponent<GameDataManager>().SoundsVolume);
        }
    }

    public bool MoveDirection(Directions direction)
    {
        int targetPositionX = posX;
        int targetPositionY = posY;
        int tileInfo = 0;
        switch (direction)
        {
            case Directions.up:
                targetPositionY = targetPositionY + 1;
                tileInfo = currentMap.IsTilePassable(posX, posY, 0);
                if (tileInfo == 0)
                {
                    currentMap.SwitchElements(posX, posY, targetPositionX, targetPositionY);
                    posY = posY + 1;
                    animator.Play("HeroMoveUp");
                    OnCharacterMoved();
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
                targetPositionY = targetPositionY - 1;
                tileInfo = currentMap.IsTilePassable(posX, posY, Directions.down);

                if (tileInfo==0)
                {
                    currentMap.SwitchElements(posX, posY, targetPositionX, targetPositionY);
                    posY = posY - 1;
                    animator.Play("HeroMoveDown");
                    OnCharacterMoved();
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
                    posX = posX + 1;
                    animator.Play("HeroMoveRight");
                    OnCharacterMoved();
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
                    animator.Play("HeroMoveLeft");
                    OnCharacterMoved();
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

    public void OnPlayerInteract(PlayerCharacter source)
    {
        OnTakenDamage(source.AttackValue);
    }

    public override void InitializePosition(int posX, int posY, Map currentMap, TurnManager turnManager)
    {
        this.posX = posX;
        this.posY = posY;
        this.currentMap = currentMap;
        this.turnManager = turnManager;
    }

    public override void OnDeath()
    {
        turnManager.RemoveEnemyFromList(this);
        Destroy(gameObject);
    }

    public void TurnVisible()
    {
        spriteRenderer.enabled = true;
        enemyHealthRepresentation.UpdateEnemyHealth(HealthPoints);
    }

    public void TurnInvisible()
    {
        enemyHealthRepresentation.HideAllElements();
        spriteRenderer.enabled = false;
        if (memoryTurns > 0) memoryTurns--;
        else
        {
            lastHeroPosX = -1;
            lastHeroPosY = -1;
        }
    }

    public void SetHeroLastPosition(int x, int y)
    {
        memoryTurns = 3;
        lastHeroPosX = x;
        lastHeroPosY = y;
    }
}
