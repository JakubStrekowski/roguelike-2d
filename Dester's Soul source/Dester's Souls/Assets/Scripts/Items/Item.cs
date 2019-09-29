using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour, IPlayerStandardInteraction, IPositionInitializer, IVisibility
{

    protected int posX;
    protected int posY;

    public int itemValue;

    protected SpriteRenderer spriteRenderer;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        TurnInvisible();
    }

    public void OnPlayerInteract(PlayerCharacter source)
    {
        OnItemPickup(source);
    }

    protected virtual void OnItemPickup(PlayerCharacter source)
    {
        if(source.isEnoughRoomInEq())
        {
            source.AddItemToEquipment(this);
            OnItemPickedUpEvent(source);
            Destroy(gameObject);
        }
    }

    protected virtual void OnItemPickedUpEvent(PlayerCharacter source)
    {

    }

    public void InitializePosition(int x, int y, Map currentMap, TurnManager tm)
    { 
        posX = x;
        posY = y;
    }

    public void TurnVisible()
    {
        spriteRenderer.enabled = true;
    }

    public void TurnInvisible()
    {
        spriteRenderer.enabled = false;
    }
}
