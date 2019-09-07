using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{
    bool directionHorizontal; //does zombie move left-right or up-down
    public bool DirectionHorizontal
    {
        get { return directionHorizontal; }
    }
    System.Random rnd;
    ZombieMoveState movementState;
    public ZombieMoveState MovementState
    {
        set { movementState = value; }
    }

    private void Start()
    {
        name = "Zombie";
        rnd = new System.Random(posX * posY);
        HealthPoints = 3;
        AttackValue = 1;

        int random = rnd.Next(2);
        if (random == 0)
        {
            directionHorizontal = true;
        }
        else
        {
            directionHorizontal = false;
        }
        if (rnd.Next(2) == 0) movementState = new MoveDownState();
        else movementState = new MoveUpState();
    }

    public override void MovementBehaviour()
    {
        movementState.ZombieMove(this);
    }
}
