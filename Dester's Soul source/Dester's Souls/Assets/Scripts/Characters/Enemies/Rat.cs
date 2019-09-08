using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : Enemy
{
    private Directions[] movementSequence;
    int currentMoveState = 0;
    void Start()
    {
        name = "Rat";
        movementSequence = new Directions[8] { Directions.up, Directions.up, Directions.right,
            Directions.right, Directions.down, Directions.down, Directions.left, Directions.left };
        HealthPoints = 1;
        AttackValue = 1;

    }
    public override void MovementBehaviour()
    {
        MoveDirection(movementSequence[currentMoveState]);
        currentMoveState = (currentMoveState + 1) % 8;
    }
}
