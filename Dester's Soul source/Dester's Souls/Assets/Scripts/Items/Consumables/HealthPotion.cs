using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Consumable
{
    public int healAmmount;
    public AudioClip onItemUseSnd;

    public override void UseItem(Character source)
    {
        source.HealthPoints += healAmmount;
        numberOfUses--;
        AudioSource.PlayClipAtPoint(onItemUseSnd, turnManager.mainCamera.transform.position, GameManager._instance.GetComponent<GameDataManager>().SoundsVolume);
    }
}
