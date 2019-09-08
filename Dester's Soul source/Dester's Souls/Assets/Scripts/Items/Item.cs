using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour, IPlayerStandardInteraction, IPositionInitializer
{

    protected int posX;
    protected int posY;

    public int itemValue;

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
        }
    }

    protected virtual void OnItemPickedUpEvent(PlayerCharacter source)
    {

    }

    public void InitializePosition(int x, int y, Map currentMap)
    {
        posX = x;
        posY = y;
    }
}
