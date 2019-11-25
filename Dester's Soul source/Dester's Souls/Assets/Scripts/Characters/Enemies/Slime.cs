using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    private Directions[] movementSequence;
    int currentMoveState = 0;
    bool waitOnce;
    public void Start()
    {
        waitOnce = true;
        name = "Slime";
        movementSequence = new Directions[4] { Directions.down, Directions.left, Directions.up, Directions.right };
        HealthPoints = 3;
        AttackValue = 1;
        enemyHealthRepresentation.maxHealth = HealthPoints;
    }
    public override void MovementBehaviour()
    {
        if (!waitOnce)
        {
            MoveDirection(movementSequence[currentMoveState]);
            currentMoveState = (currentMoveState + 1) % 4;
        }
        waitOnce = !waitOnce;
    }
}
