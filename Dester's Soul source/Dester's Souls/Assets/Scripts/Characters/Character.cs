using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour, IPositionInitializer
{
    public enum Directions
    {
        up, down, right, left
    }

    private int _healthPoints;
    private int _attackValue;

    public int HealthPoints { get => _healthPoints;
        set => _healthPoints = value; }
    public int AttackValue { get => _attackValue; protected set => _attackValue = value; }

    protected int posX;
    protected int posY;

    protected Map currentMap;

    private Item[] equipment;
    // Start is called before the first frame update
    void Awake()
    {
        equipment = new Item[5];
    }

    // Update is called once per frame

    public virtual void OnDeath()
    {
        //TODO
    }

    public void OnTakenDamage(int damageAmmount)
    {

    }

    public void InitializePosition(int posX, int posY,Map currentMap)
    {
        this.posX = posX;
        this.posY = posY;
        this.currentMap = currentMap;
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
}
