using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdle : SkeletonMoveState
{

    public override void SkeletonInit(Skeleton context)
    {
    }

    public override void SkeletonMove(Skeleton context)
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
            context.moveState = new SkeletonChasing();
        }
    }
}
