using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Image itemSlot;
    public Image item;
    // Start is called before the first frame update

    private void Start()
    {
        DisableItem();
    }
    public void EnableItem(Sprite itemPresentation)
    {
        item.sprite = itemPresentation;
        item.enabled = true;
        itemSlot.color = new Color(itemSlot.color.r, itemSlot.color.g, itemSlot.color.b, 1f);
    }

    public void DisableItem()
    {
        item.enabled = false;
        itemSlot.color = new Color(itemSlot.color.r, itemSlot.color.g, itemSlot.color.b, 0.2f);
    }
}
