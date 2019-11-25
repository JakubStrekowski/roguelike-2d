using System.Collections;
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

    public int posX;
    public int posY;

    protected Animator animator;
    protected CharacterEffectPlayer characterEffect;
    public GameObject particleSystem;

    public Map currentMap;
    protected TurnManager turnManager;

    public Item[] equipment;
    // Start is called before the first frame update
    void Awake()
    {
        equipment = new Item[6];
    }

    // Update is called once per frame

    public virtual void OnDeath()
    {
        GameObject.Destroy(gameObject);
    }

    public virtual void OnTakenDamage(int damageAmmount)
    {
        HealthPoints -= damageAmmount;
        characterEffect.PlayHurtAnimation();
        StartCoroutine("StartBleeding");
        if (HealthPoints <= 0)
        {
            OnDeath();
        }
    }

    protected IEnumerator StartBleeding()
    {
        Instantiate(particleSystem, this.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.1f);
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
        StartCoroutine("AwaitForAnimation");
    }

    IEnumerator AwaitForAnimation()
    {
        yield return new WaitForSeconds(0.20f);
        gameObject.transform.position = new Vector3(posX, posY, gameObject.transform.position.z);
        animator.Play("HeroIdle");
        gameObject.transform.GetChild(0).position = Vector3.zero;
        
    }
}
