using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static EnemyPathFindingAStar;

public class RatChase : RatMoveState
{
    public override void RatInit(Rat context)
    {

    }

    public override void RatMove(Rat context)
    {
        if (context.alliesNearby == 0)
        {
            context.currentMoveState = new RatRunAway();
        }
        if (context.LastHeroPosX == -1)
        {
            context.currentMoveState = new RatIdle();
        }
        else
        {
            LevelGenerator.Point startPoint = new LevelGenerator.Point();
            startPoint.X = 5;
            startPoint.Y = 5;
            LevelGenerator.Point endPoint = new LevelGenerator.Point();
            endPoint.X = context.LastHeroPosX - context.posX + 5;
            endPoint.Y = context.LastHeroPosY - context.posY + 5;
            List<Node> path = null;
            Stopwatch sw;

            if (GameManager._instance.GetComponent<GameDataManager>().DebugConsoleEnabled == 1)
            {
                if (GameManager._instance.GetComponent<GameDataManager>().UsingBreadthAlgorithm == 1)
                {
                    sw = Stopwatch.StartNew();

                    for (int i = 0; i < 100; i++)
                    {
                        if (context.currentMap.enemyPathFindingA.FindPathBreadth(startPoint, endPoint, context.currentMap.GenerateTruePathCosts(context.posX - 5, context.posY - 5, context.posX + 5, context.posY + 5, context), 5, 5) is null)
                        {
                            return;
                        }
                    }

                    sw.Stop();
                    context.GetComponent<Enemy>().turnManager.AddLog("Enemy " + context.gameObject.GetInstanceID().ToString() + " found path with BFS in: " + sw.Elapsed.Ticks + " ticks", sw.Elapsed.Ticks.ToString() + " ");
                }

                sw = Stopwatch.StartNew();
                for (int i = 0; i < 100; i++)
                {
                    path = context.currentMap.enemyPathFindingA.FindPath(startPoint, endPoint, context.currentMap.GenerateTruePathCosts(context.posX - 5, context.posY - 5, context.posX + 5, context.posY + 5, context), 5, 5);
                }
                if (path is null)
                {
                    return;
                }
                sw.Stop();
                context.GetComponent<Enemy>().turnManager.AddLog("Enemy " + context.gameObject.GetInstanceID().ToString() + " found path in: " + sw.Elapsed.Ticks + " ticks", sw.Elapsed.Ticks.ToString() + "\n"); //10000 ticks is 1 ms
            }
            else
            {
                for (int i = 0; i < 100; i++)
                {
                    path = context.currentMap.enemyPathFindingA.FindPath(startPoint, endPoint, context.currentMap.GenerateTruePathCosts(context.posX - 5, context.posY - 5, context.posX + 5, context.posY + 5, context), 5, 5);
                }
                if (path is null)
                {
                    return;
                }
            }

            int subtractAmmount;
            if (path.Count > 1) subtractAmmount = 2;
            else subtractAmmount = 1;
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
            }
            
        }
    }
}
