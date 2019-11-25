using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthRepresentation : MonoBehaviour
{

    public EnemyHeartContainer[] heartContainers;

    public int maxHealth;

    public void UpdateEnemyHealth(int currentHealth)
    {
        if (currentHealth != maxHealth)
        {
            for (int i = 0; i < currentHealth; i++)
            {
                heartContainers[i].Activate();
            }
            for (int i = currentHealth; i < heartContainers.Length; i++)
            {
                heartContainers[i].Deactivate();
            }
        }
    }

    public void HideAllElements()
    {
        foreach(EnemyHeartContainer ehr in heartContainers)
        {
            ehr.DeactivateFast();
        }
    }

}
