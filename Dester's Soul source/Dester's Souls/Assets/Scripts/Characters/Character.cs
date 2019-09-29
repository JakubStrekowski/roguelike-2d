﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour, IPositionInitializer
{
    public enum Directions
    {
        up, down, right, left
    }

    protected int _healthPoints;
    private int _attackValue;
    
    public virtual int HealthPoints { get => _healthPoints;
        set => _healthPoints = value; }
    public int AttackValue { get => _attackValue; protected set => _attackValue = value; }

    protected int posX;
    protected int posY;

    protected Map currentMap;
    protected TurnManager turnManager;

    protected Item[] equipment;
    // Start is called before the first frame update
    void Awake()
    {
        equipment = new Item[5];
    }

    // Update is called once per frame

    public virtual void OnDeath()
    {
        GameObject.Destroy(gameObject);
    }

    public void OnTakenDamage(int damageAmmount)
    {
        HealthPoints -= damageAmmount;
        if (HealthPoints <= 0)
        {
            OnDeath();
        }
    }

    public virtual void InitializePosition(int posX, int posY,Map currentMap,TurnManager turnManager)
    {
        this.posX = posX;
        this.posY = posY;
        this.currentMap = currentMap;
        this.turnManager = turnManager;
    }

    public bool isEnoughRoomInEq()
    {
        foreach(Item it in equipment)
        {
            if (it is null) return true;
        }
        return false;
    }

    public void AddItemToEquipment(Item item)
    {
        for(int i=0;i<equipment.Length;i++)
        {
            if (equipment[i] != null) continue;
            else
            {
                equipment[i] = item;
                return;
            }
        }
    }

    protected void OnCharacterMoved()
    {
        gameObject.transform.position = new Vector3(posX, posY, gameObject.transform.position.z);
    }
}
