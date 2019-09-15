using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static LevelGenerator;

public class AStarCorridors
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
        public int CalculateHeuristicVal(Point targetPos,int myCost)
        {
            heuristicValue= FunctionF(targetPos) + myCost;
            return heuristicValue;
        }

        public int FunctionF(Point targetPos)
        {
            Point distancePoint = LevelGenerator.MeasureDistance(position, targetPos);
            return distancePoint.X + distancePoint.Y;
        }

        public Node[] GenerateSuccessors(int[][]costTable)
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
            for(int i = 0; i < successess; i++)
            {
                successors[i] = potentialNodes[i];
            }
            return successors;
        }
    }

    List<Node> openNodes;
    List<Node> closedNodes;
    int[][] costTable;

    public AStarCorridors(int sizeX, int sizeY)
    {
        openNodes = new List<Node>();
        closedNodes = new List<Node>();
        costTable = new int[sizeY][];
        for (int i = 0; i < sizeY; i++)
        {
            costTable[i] = new int[sizeX];
            for (int j = 0; j < sizeX; j++)
            {
                costTable[i][j] = 1;
            }
        }
    }

    private void ResetCostValues()
    {
        openNodes = new List<Node>();
        closedNodes = new List<Node>();
        for (int i = 0; i < costTable.Length; i++)
        {
            for (int j = 0; j < costTable[0].Length; j++)
            {
                costTable[i][j] = 2;
            }
        }
    }

    public List<Node> ConnectTwoRoomsNoCollisions(int[][] dungeon,List<Room>allRooms, Room sourceRoom, Room targetRoom)
    {
        ResetCostValues();
        SetCostValuesNoCollisions(allRooms, sourceRoom, targetRoom,dungeon);
        Point sourcePoint = sourceRoom.RandomPointFromRoom();
        Point targetPoint = targetRoom.RandomPointFromRoom();
        
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
            Node[] successors = currentNode.GenerateSuccessors(costTable);
            if (successors is null) continue;
            foreach (Node successorNode in successors)
            {
                if (successorNode.position.X == 0 || successorNode.position.Y == 0 
                    || successorNode.position.X == dungeon[0].Length || successorNode.position.Y == dungeon.Length) continue;

                if (closedNodes.Exists(x => (x.position.X == successorNode.position.X) && (x.position.Y == successorNode.position.Y))) continue;

                else
                {
                    int possibleNewRouteVal = currentNode.totalCost
                    + costTable[successorNode.position.Y][successorNode.position.X];
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
        int[][] debugMap=new int[costTable.Length][];
        for(int i = 0; i < debugMap.Length; i++)
        {
            debugMap[i] = new int[costTable[0].Length];
            for(int j = 0; j < debugMap[0].Length; j++)
            {
                debugMap[i][j] = 0;
            }
        }
        foreach(Node node in closedNodes)
        {
            debugMap[node.position.Y][node.position.X] = 1;
        }
        for (int i = 0; i < debugMap.Length; i++)
        {
            string s = "";
            for (int j = 0; j < debugMap[0].Length; j++)
            {
                s += debugMap[i][j].ToString()+" ";
            }
            //Debug.Log(s);
        }

        Debug.Log("Couldn't connect: "+sourcePoint.Y+" "+sourcePoint.X+" to: "+targetPoint.Y+" "+targetPoint.X);
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

    private void SetCostValuesNoCollisions(List<Room> allRooms, Room sourceRoom, Room targetRoom, int[][] dungeon)
    {
        int i = 0;
        int j = 0;
        //assigning cost to every tile
        for (i = 0; i < dungeon.Length; i++)
        {
            for (j = 0; j < dungeon[0].Length; j++)
            {
                if (dungeon[i][j] <= 9) //we met floor
                {
                    costTable[i][j] = 1;
                }
                else if (dungeon[i][j] <= 19)//we met wall
                {
                    costTable[i][j] = 6;
                }
            }
        }

        //assigning cost values for walls and floors in all rooms (we dont want corridors to pass unwanted rooms)
        foreach (Room room in allRooms)
        {
            for (i = room.beginY; i < room.beginY + room.sizeY; i++)
            {
                for (j = room.beginX; j < room.beginX + room.sizeX; j++)
                {
                    if (j == room.beginX || j == room.beginX + room.sizeX -1 || i == room.beginY || i == room.beginY + room.sizeY-1)
                    {
                        costTable[i][j] = 999;
                    }
                    else
                    {
                        costTable[i][j] = 1;
                    }
                }
            }
        }
        //assigning cost values for walls and floors, allowing to pass source floor
        for (i = sourceRoom.beginY; i < sourceRoom.beginY + sourceRoom.sizeY; i++)
        {
            for (j = sourceRoom.beginX; j < sourceRoom.beginX + sourceRoom.sizeX; j++)
            {
                if(j == sourceRoom.beginX||j== sourceRoom.beginX + sourceRoom.sizeX-1||i== sourceRoom.beginY|| i==sourceRoom.beginY + sourceRoom.sizeY-1)
                {
                    costTable[i][j] = 10;
                }
                else
                {
                    costTable[i][j] = 1;
                }
            }
        }
        //assigning cost values for walls and floors, allowing to pass target floor
        for (i = targetRoom.beginY; i < targetRoom.beginY + targetRoom.sizeY; i++)
        {
            for (j = targetRoom.beginX; j < targetRoom.beginX + targetRoom.sizeX; j++)
            {
                if (j == targetRoom.beginX || j == targetRoom.beginX + targetRoom.sizeX -1|| i == targetRoom.beginY || i == targetRoom.beginY + targetRoom.sizeY-1)
                {
                    costTable[i][j] = 10;
                }
                else
                {
                    costTable[i][j] = 1;
                }
            }
        }
    }
}
