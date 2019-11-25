using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LevelGenerator;

public class EnemyPathFindingAStar
{
    public class Node
    {
        public Node previousNode;
        public Point position;
        public int totalCost;
        public int heuristicValue;
        public Node(Point position)
        {
            previousNode = null;
            totalCost = int.MaxValue;
            heuristicValue = int.MaxValue;
            this.position = position;
        }
        public int CalculateHeuristicVal(Point targetPos, int myCost)
        {
            heuristicValue = FunctionF(targetPos) + myCost;
            return heuristicValue;
        }

        public int FunctionF(Point targetPos)
        {
            Point distancePoint = LevelGenerator.MeasureDistance(position, targetPos);
            return distancePoint.X + distancePoint.Y;
        }

        public Node[] GenerateSuccessors(int[][] costTable)
        {
            Node[] potentialNodes = new Node[4];
            int successess = 0;
            if (position.X - 1 > 0)
            {
                Point newPos = new Point();
                newPos.X = position.X - 1;
                newPos.Y = position.Y;
                potentialNodes[successess] = new Node(newPos);
                successess++;
            }
            if (position.X + 1 < costTable[0].Length)
            {
                Point newPos = new Point();
                newPos.X = position.X + 1;
                newPos.Y = position.Y;
                potentialNodes[successess] = new Node(newPos);
                successess++;
            }
            if (position.Y - 1 > 0)
            {
                Point newPos = new Point();
                newPos.X = position.X;
                newPos.Y = position.Y - 1;
                potentialNodes[successess] = new Node(newPos);
                successess++;
            }
            if (position.Y + 1 < costTable.Length)
            {
                Point newPos = new Point();
                newPos.X = position.X;
                newPos.Y = position.Y + 1;
                potentialNodes[successess] = new Node(newPos);
                successess++;
            }
            if (successess == 0) return null;
            Node[] successors = new Node[successess];
            for (int i = 0; i < successess; i++)
            {
                successors[i] = potentialNodes[i];
            }
            return successors;
        }
    }

    List<Node> openNodes;
    List<Node> closedNodes;
    int sizeX;
    int sizeY;
    public EnemyPathFindingAStar( int sizeX, int sizeY)
    {
        openNodes = new List<Node>();
        closedNodes = new List<Node>();

    }

    private void ResetCostValues()
    {
        openNodes.Clear();
        closedNodes.Clear();
    }

    public List<Node> FindPath(Point sourcePoint, Point targetPoint, int[][] costs, int offsetX, int offsetY)
    {
        ResetCostValues();

        openNodes.Add(new Node(sourcePoint));
        openNodes[0].totalCost = 0;
        openNodes[0].heuristicValue = openNodes[0].FunctionF(targetPoint);
        while (openNodes.Count != 0)
        {
            Node currentNode = null;
            int minimalCost = int.MaxValue;
            foreach (Node node in openNodes)
            {
                if (minimalCost >= node.heuristicValue)
                {
                    minimalCost = node.heuristicValue;
                    currentNode = node;
                }
            }
            openNodes.Remove(currentNode);
            closedNodes.Add(currentNode);
            if (currentNode.position.Y == targetPoint.Y && currentNode.position.X == targetPoint.X)
            {
                return RecreatePath(currentNode);
            }
            //generating succesor nodes
            Node[] successors = currentNode.GenerateSuccessors(costs);
            if (successors is null) continue;
            foreach (Node successorNode in successors)
            {
                if (successorNode.position.X == 0 || successorNode.position.Y == 0
                    || successorNode.position.X == sizeX || successorNode.position.Y == sizeY) continue;

                if (closedNodes.Exists(x => (x.position.X == successorNode.position.X) && (x.position.Y == successorNode.position.Y))) continue;

                else
                {
                    int possibleNewRouteVal = currentNode.totalCost
                    + costs[successorNode.position.Y][successorNode.position.X];
                    if (possibleNewRouteVal < successorNode.totalCost)
                    {
                        successorNode.previousNode = currentNode;
                        successorNode.totalCost = possibleNewRouteVal;
                        successorNode.heuristicValue = successorNode.CalculateHeuristicVal(targetPoint, successorNode.totalCost);
                        if (!(openNodes.Exists(x => (x.position.X == successorNode.position.X) && (x.position.Y == successorNode.position.Y))))
                        {
                            openNodes.Add(successorNode);
                        }
                    }
                }
            }
        }
        int[][] debugMap = new int[costs.Length][];
        for (int i = 0; i < debugMap.Length; i++)
        {
            debugMap[i] = new int[costs[0].Length];
            for (int j = 0; j < debugMap[0].Length; j++)
            {
                debugMap[i][j] = 0;
            }
        }
        foreach (Node node in closedNodes)
        {
            debugMap[node.position.Y][node.position.X] = 1;
        }
        for (int i = 0; i < debugMap.Length; i++)
        {
            string s = "";
            for (int j = 0; j < debugMap[0].Length; j++)
            {
                s += debugMap[i][j].ToString() + " ";
            }
            //Debug.Log(s);
        }

        Debug.Log("Couldn't connect: " + sourcePoint.Y + " " + sourcePoint.X + " to: " + targetPoint.Y + " " + targetPoint.X);
        return null;
    }

    private List<Node> RecreatePath(Node endNode)
    {
        Node currentNode = endNode;
        List<Node> path = new List<Node>();
        while (currentNode != null)
        {
            path.Add(currentNode);
            currentNode = currentNode.previousNode;
        }
        return path;
    }
}
