using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyPathFindingAStar;

public class SkeletonChasing : SkeletonMoveState
{
    int moveBeforeRest;
    public override void SkeletonInit(Skeleton context)
    {
        moveBeforeRest = 3;
    }

    public override void SkeletonMove(Skeleton context)
    {
        if (context.LastHeroPosX == -1)
        {
            context.moveState = new SkeletonIdle();
        }
        else
        {
            LevelGenerator.Point startPoint = new LevelGenerator.Point();
            startPoint.X = 5;
            startPoint.Y = 5;
            LevelGenerator.Point endPoint = new LevelGenerator.Point();
            endPoint.X = context.LastHeroPosX - context.posX + 5;
            endPoint.Y = context.LastHeroPosY - context.posY + 5;
            List<Node>path = context.currentMap.enemyPathFindingA.FindPath(startPoint, endPoint, context.currentMap.GenerateTruePathCosts(context.posX - 5, context.posY - 5, context.posX + 5, context.posY + 5, context), 5, 5);
            if(path is null)
            {
                return;
            }
            int subtractAmmount;
            if (path.Count > 1) subtractAmmount = 2;
            else subtractAmmount = 1;
            if (moveBeforeRest == 0)
            {
                moveBeforeRest = 3;
            }
            else
            {
                if (path.Count > 1)
                {
                    if (path[path.Count - subtractAmmount].position.X > startPoint.X)
                    {
                        context.MoveDirection(Character.Directions.right);
                    }
                    if (path[path.Count - subtractAmmount].position.X < startPoint.X)
                    {
                        context.MoveDirection(Character.Directions.left);
                    }
                    if (path[path.Count - subtractAmmount].position.Y > startPoint.Y)
                    {
                        context.MoveDirection(Character.Directions.up);
                    }
                    if (path[path.Count - subtractAmmount].position.Y < startPoint.Y)
                    {
                        context.MoveDirection(Character.Directions.down);
                    }
                    moveBeforeRest--;
                }
            }
        }
    }

}
