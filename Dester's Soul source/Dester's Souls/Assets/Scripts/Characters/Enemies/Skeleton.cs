using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    public SkeletonMoveState moveState;
    int currentMoveState = 0;
    private void Start()
    {
        name = "Skeleton";
        HealthPoints = 2;
        AttackValue = 1;
        enemyHealthRepresentation.maxHealth = HealthPoints;
        moveState = new SkeletonIdle();
    }
    public override void MovementBehaviour()
    {

        moveState.SkeletonMove(this);
    }


    
}
