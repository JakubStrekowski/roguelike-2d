using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyPathFindingAStar;

public class RatRunAway : RatMoveState
{
    int moveBeforeRest;
    public override void RatInit(Rat context)
    {
        moveBeforeRest = 3;
    }

    public override void RatMove(Rat context)
    {
        context.currentMap.GenerateTruePathCosts(context.posX - 5, context.posY - 5, context.posX + 5, context.posY + 5, context);
        if (context.alliesNearby > 0)
        {
            context.currentMoveState = new RatChase();
        }
        if (context.LastHeroPosX == -1)
        {
            context.currentMoveState = new RatIdle();
        }
        else
        {
            LevelGenerator.Point startPoint = new LevelGenerator.Point();
            startPoint.X = context.posX;
            startPoint.Y = context.posY;
            LevelGenerator.Point endPoint = new LevelGenerator.Point();
            endPoint.X = context.LastHeroPosX;
            endPoint.Y = context.LastHeroPosY;

            if (startPoint.X < endPoint.X)
            {
                context.MoveDirection(Character.Directions.left);
                return;
            }
            if (startPoint.X > endPoint.X)
            {
                context.MoveDirection(Character.Directions.right);
                return;
            }
            if (startPoint.Y < endPoint.Y)
            {
                context.MoveDirection(Character.Directions.down);
                return;
            }
            if (startPoint.Y > endPoint.Y)
            {
                context.MoveDirection(Character.Directions.up);
                return;
            }

        }
    }
}
