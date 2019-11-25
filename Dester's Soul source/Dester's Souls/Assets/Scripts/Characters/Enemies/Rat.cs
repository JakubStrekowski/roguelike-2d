using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : Enemy
{
    public RatMoveState currentMoveState;

    public int alliesNearby = 0;
    void Start()
    {
        name = "Rat";
        HealthPoints = 1;
        AttackValue = 1;
        enemyHealthRepresentation.maxHealth = HealthPoints;
        alliesNearby = 0;
        currentMoveState = new RatIdle();
    }
    public override void MovementBehaviour()
    {
        currentMoveState.RatMove(this);
    }
}
