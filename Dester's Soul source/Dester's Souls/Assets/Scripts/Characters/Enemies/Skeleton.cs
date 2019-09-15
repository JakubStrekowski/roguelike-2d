using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    private Directions[] movementSequence;
    int currentMoveState = 0;
    private void Start()
    {
        name = "Skeleton";
        movementSequence = new Directions[4] { Directions.right, Directions.left, Directions.up, Directions.down };
        HealthPoints = 2;
        AttackValue = 1;
    }
    public override void MovementBehaviour()
    {

        MoveDirection(movementSequence[currentMoveState]);
        currentMoveState = (currentMoveState + 1) % 4;
    }


    
}
