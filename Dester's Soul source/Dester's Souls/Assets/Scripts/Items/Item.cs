using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour, IPlayerStandardInteraction, IPositionInitializer, IVisibility
{
    public AudioClip onItemPickupSnd;

    protected int posX;
    protected int posY;
    protected Map currentMap;
    protected TurnManager turnManager;

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
            currentMap.DeleteItem(posX, posY);
            transform.position = new Vector3(-10000, -10000);
            AudioSource.PlayClipAtPoint(onItemPickupSnd, turnManager.mainCamera.transform.position, GameManager._instance.GetComponent<GameDataManager>().SoundsVolume);
        }
    }

    protected virtual void OnItemPickedUpEvent(PlayerCharacter source)
    {

    }

    public abstract void UseItem(Character source);

    public void InitializePosition(int x, int y, Map currentMap, TurnManager tm)
    { 
        posX = x;
        posY = y;
        this.currentMap = currentMap;
        this.turnManager = tm;
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
