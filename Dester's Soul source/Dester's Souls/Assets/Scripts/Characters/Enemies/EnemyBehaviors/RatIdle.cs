using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatIdle : RatMoveState
{
    public override void RatInit(Rat context)
    {
        throw new System.NotImplementedException();
    }

    public override void RatMove(Rat context)
    {
        if (context.LastHeroPosX == -1)
        {
            int moveChance = Random.Range(0, 10);
            if (moveChance > 5)
            {
                moveChance -= 6;
                context.MoveDirection((Character.Directions)moveChance);
            }
        }
        else
        {
            if (context.alliesNearby > 0)
            {
                context.currentMoveState = new RatChase();
            }
            else
            {
                context.currentMoveState = new RatRunAway();
            }
        }
    }
}
