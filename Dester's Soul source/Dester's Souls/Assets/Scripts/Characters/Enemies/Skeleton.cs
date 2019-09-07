using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    private int[] movementSequence;
    int currentMoveState = 0;
    private void Start()
    {
        name = "Skeleton";
        movementSequence = new int[4] { 2, 3, 0, 1 };
        HealthPoints = 2;
        AttackValue = 1;
    }
    public override void MovementBehaviour()
    {

        //moveDirection(movementSequence[currentMoveState]);
        currentMoveState = (currentMoveState + 1) % 4;
    }


    public override void OnDeath()
    {
        throw new System.NotImplementedException();
    }
}
