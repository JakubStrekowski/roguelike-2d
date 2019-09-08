using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private int[][] dungeon;
    private List<Room> rooms;
    private List<Room> connectedRooms;
    private int mapSizeX;
    private int mapSizeY;
    System.Random rnd;

    public LevelGenerator(int sizeX, int sizeY)
    {
        mapSizeX = sizeX;
        mapSizeY = sizeY;
        connectedRooms = new List<Room>();
        rooms = new List<Room>();
        rnd = new System.Random();
        dungeon = new int[sizeY][];
        for (int i = 0; i < sizeY; i++)
        {
            dungeon[i] = new int[sizeX];
            for (int j = 0; j < sizeX; j++)
            {
                dungeon[i][j] = 1;
            }
        }
    }

    public int[][] CreateDungeon(int sizeX, int sizeY, int roomsAmnt)
    {
        mapSizeX = sizeX;
        mapSizeY = sizeY;
        rooms = new List<Room>();
        rnd = new System.Random();
        dungeon = new int[sizeY][];
        for (int i = 0; i < sizeY; i++)
        {
            dungeon[i] = new int[sizeX];
            for (int j = 0; j < sizeX; j++)
            {
                dungeon[i][j] = 1;
            }
        }
        CreateIfPossible(rnd.Next(4, 8), rnd.Next(4, 8), 0, rnd.Next(10, 16), rnd.Next(10, 16), false);
        rooms[0].isConnected = true;
        connectedRooms.Add(rooms[0]);
        int selectedRoom;
        while (rooms.Count != roomsAmnt)
        {

            selectedRoom = rnd.Next(rooms.Count);

            int dir = rnd.Next(4);

            Point nextPlace;
            nextPlace.X = rnd.Next(0, mapSizeX);
            nextPlace.Y = rnd.Next(0, mapSizeY);
            CreateIfPossible(rnd.Next(6, 12), rnd.Next(6, 12), dir, nextPlace.X, nextPlace.Y, false);

        }
        int id = 0;
        foreach (Room room in rooms)
        {
            if (!room.isConnected)
            {
                int rand = rnd.Next(connectedRooms.Count);
                Room rndroom = rooms[rand];
                while (rndroom == room)
                {
                    rand = rnd.Next(rooms.Count);
                    rndroom = rooms[rand];
                }
                connectedRooms.Add(room);
                ConnectTwoRooms(room, rndroom);
                room.AddConnection(rand);
                rndroom.AddConnection(id);
                room.isConnected = true;
                rndroom.isConnected = true;
            }
            id++;
        }


        int randomConnections = rnd.Next(2, 8);
        for (int i = 0; i < randomConnections; i++)
        {
            int room1 = rnd.Next(connectedRooms.Count);
            int room2 = rnd.Next(connectedRooms.Count);
            if (room1 == room2)
            {
                room2 = rnd.Next(connectedRooms.Count);
            }
            ConnectTwoRooms(connectedRooms[room1], connectedRooms[room2]);
        }



        for (int i = 1; i < rooms.Count; i++)
        {
            int enemyAmnt = rnd.Next(2, 6);
            int goldChance = rnd.Next(3);
            int healthPotsChance = rnd.Next(10);
            for (int j = 0; j < enemyAmnt; j++)
            {
                Point pnt = rooms[i].RandomPointFromRoom();
                switch (rnd.Next(5))
                {
                    case 0: dungeon[pnt.Y][pnt.X] = 3; break;
                    case 1: dungeon[pnt.Y][pnt.X] = 6; break;
                    case 2: dungeon[pnt.Y][pnt.X] = 8; break;
                    case 3: dungeon[pnt.Y][pnt.X] = 9; break;
                }
            }
            if (goldChance == 2)
            {
                Point pnt = rooms[i].RandomPointFromRoom();
                dungeon[pnt.Y][pnt.X] = 5;
            }
            if (healthPotsChance == 9)
            {
                Point pnt = rooms[i].RandomPointFromRoom();
                dungeon[pnt.Y][pnt.X] = 7;
            }
        }

        Room lastRoom = FindFurthest(rooms[0]);
        Point randInLast = lastRoom.RandomPointFromRoom();
        dungeon[randInLast.Y][randInLast.X] = 4;
        PutHero();
        
        foreach(int[] itgr in dungeon)
        {
            string s = "";
            foreach(int integer in itgr)
            {
                s += integer.ToString();
            }
            Debug.Log(s);
        }
        return dungeon;

    }

    public void CheckIntegrity(int roomID, int distanceFromSpawn)
    {
        if (!rooms[roomID].visited)
        {
            int checkedRoom = roomID;
            rooms[roomID].visited = true;
            connectedRooms.Add(rooms[roomID]);
            distanceFromSpawn++;
            foreach (int connectedRoom in rooms[checkedRoom].connectedRooms)
            {
                CheckIntegrity(connectedRoom, distanceFromSpawn);
            }
        }

    }

    private bool CreateIfPossible(int sizeX, int sizeY, int direction, int beginX, int beginY, bool corridor)
    {
        int sourceX = beginX;
        int sourceY = beginY;
        bool possible = true;
        switch (direction)
        {
            case 0:
                beginY -= (sizeY - 1);
                beginX = beginX + rnd.Next(-(sizeX - 2) / 2, (sizeX - 2) / 2);
                break;
            case 1:
                beginY -= 1;
                beginX = beginX + rnd.Next(-((sizeX - 2) / 2), ((sizeX - 2) / 2));
                break;
            case 2:
                beginX += 1;
                beginY = beginY + rnd.Next(-(sizeY - 2) / 2, (sizeY - 2) / 2);
                break;
            case 3:
                beginX -= (sizeX - 1);
                beginY = beginY + rnd.Next(-((sizeX - 2) / 2), ((sizeX - 2) / 2));
                break;
            default:
                break;
        }
        if (!(beginX >= 0) || !(beginY >= 0) || sizeX + beginX > mapSizeX || sizeY + beginY > mapSizeY)
        {
            return false;
        }
        for (int i = beginY; i < sizeY + beginY; i++)
        {
            for (int j = beginX; j < sizeX + beginX; j++)
            {
                if (dungeon[i][j] != 1)
                {
                    return false;
                }
            }
        }
        if (!possible) return false;
        for (int i = beginY; i < sizeY + beginY; i++)
        {
            for (int j = beginX; j < sizeX + beginX; j++)
            {
                if (!corridor && (i == 0 || j == 0 || i == beginY + sizeY - 1 || j == beginX + sizeX - 1))
                {
                    dungeon[i][j] = 1;
                }
                else
                {
                    dungeon[i][j] = 0;
                }
            }
        }
        if (!corridor) rooms.Add(new Room(beginX, sizeX, beginY, sizeY));
        return true;
    }


    private Room FindFurthest(Room source)
    {
        int maxDistance = 0;
        Room furthestRoom = source;
        Point sourcePt = source.RandomPointFromRoom();
        for (int i = 1; i < rooms.Count; i++)
        {
            Point distPt = MeasureDistance(sourcePt, rooms[i].RandomPointFromRoom());
            if (distPt.X + distPt.Y > maxDistance)
            {
                maxDistance = distPt.X + distPt.Y;
                furthestRoom = rooms[i];
            }
        }
        return furthestRoom;
    }


    private void ConnectTwoRooms(Room room1, Room room2)
    {
        Point currentPoint;
        Point pointRoom1;
        Point pointRoom2;
        pointRoom1.X = rnd.Next(room1.beginX + 1, room1.beginX + room1.sizeX - 1);
        pointRoom1.Y = rnd.Next(room1.beginY + 1, room1.beginY + room1.sizeY - 1);
        pointRoom2.X = rnd.Next(room2.beginX + 1, room2.beginX + room2.sizeX - 1);
        pointRoom2.Y = rnd.Next(room2.beginY + 1, room2.beginY + room2.sizeY - 1);
        currentPoint.X = pointRoom2.X;
        currentPoint.Y = pointRoom2.Y;
        int distanceX = MeasureDistance(currentPoint, pointRoom1).X;
        int distanceY = MeasureDistance(currentPoint, pointRoom1).Y;
        bool upFirst = (rnd.Next(2) == 1) ? true : false;

        Point possibleRoute;
        bool madeStep = false;
        while (!(distanceX == 0 && distanceY == 0))
        {
            madeStep = false;
            possibleRoute.X = currentPoint.X;
            possibleRoute.Y = currentPoint.Y;
            if (upFirst)
            {
                possibleRoute.Y -= 1;
                if (MeasureDistance(pointRoom1, possibleRoute).Y < distanceY && !madeStep)
                {
                    distanceY = MeasureDistance(pointRoom1, possibleRoute).Y;
                    dungeon[possibleRoute.Y][possibleRoute.X] = 0;
                    currentPoint = possibleRoute;
                    madeStep = true;
                }
                else
                {
                    possibleRoute.Y += 2;
                    if (MeasureDistance(pointRoom1, possibleRoute).Y < distanceY && !madeStep)
                    {
                        distanceY = MeasureDistance(pointRoom1, possibleRoute).Y;
                        dungeon[possibleRoute.Y][possibleRoute.X] = 0;
                        currentPoint = possibleRoute;
                        madeStep = true;
                    }
                }
                possibleRoute.Y -= 1;
                possibleRoute.X -= 1;
                if (MeasureDistance(pointRoom1, possibleRoute).X < distanceX && !madeStep)
                {
                    distanceX = MeasureDistance(pointRoom1, possibleRoute).X;
                    dungeon[possibleRoute.Y][possibleRoute.X] = 0;
                    currentPoint = possibleRoute;
                    madeStep = true;
                }
                else
                {
                    possibleRoute.X += 2;
                    if (MeasureDistance(pointRoom1, possibleRoute).X < distanceX && !madeStep)
                    {
                        distanceX = MeasureDistance(pointRoom1, possibleRoute).X;
                        dungeon[possibleRoute.Y][possibleRoute.X] = 0;
                        currentPoint = possibleRoute;
                        madeStep = true;
                    }
                }
                possibleRoute.X -= 1;
            }
            else
            {
                possibleRoute.X -= 1;
                if (MeasureDistance(pointRoom1, possibleRoute).X < distanceX && !madeStep)
                {
                    distanceX = MeasureDistance(pointRoom1, possibleRoute).X;
                    dungeon[possibleRoute.Y][possibleRoute.X] = 0;
                    currentPoint = possibleRoute;
                    madeStep = true;
                }
                else
                {
                    possibleRoute.X += 2;
                    if (MeasureDistance(pointRoom1, possibleRoute).X < distanceX && !madeStep)
                    {
                        distanceX = MeasureDistance(pointRoom1, possibleRoute).X;
                        dungeon[possibleRoute.Y][possibleRoute.X] = 0;
                        currentPoint = possibleRoute;
                        madeStep = true;
                    }
                }
                possibleRoute.X -= 1;
                possibleRoute.Y -= 1;
                if (MeasureDistance(pointRoom1, possibleRoute).Y < distanceY && !madeStep)
                {
                    distanceY = MeasureDistance(pointRoom1, possibleRoute).Y;
                    dungeon[possibleRoute.Y][possibleRoute.X] = 0;
                    currentPoint = possibleRoute;
                    madeStep = true;
                }
                else
                {
                    possibleRoute.Y += 2;
                    if (MeasureDistance(pointRoom1, possibleRoute).Y < distanceY && !madeStep)
                    {
                        distanceY = MeasureDistance(pointRoom1, possibleRoute).Y;
                        dungeon[possibleRoute.Y][possibleRoute.X] = 0;
                        currentPoint = possibleRoute;
                        madeStep = true;
                    }
                }
                possibleRoute.Y -= 1;
            }

            if (rnd.Next(1, 101) > 90)
            {
                upFirst = !upFirst;
            }
        }
    }


    private Point MeasureDistance(Point a, Point b)
    {
        Point distance;
        if (a.X < b.X)
        {
            distance.X = b.X - a.X;
        }
        else
        {
            distance.X = a.X - b.X;
        }
        if (a.Y < b.Y)
        {
            distance.Y = b.Y - a.Y;
        }
        else
        {
            distance.Y = a.Y - b.Y;
        }
        return distance;
    }

    private void PutHero()
    {
        int posX = rnd.Next(rooms[0].beginX + 1, rooms[0].beginX + rooms[0].sizeX - 1);
        int posY = rnd.Next(rooms[0].beginY + 1, rooms[0].beginY + rooms[0].sizeY - 1);
        dungeon[posY][posX] = 2;
    }

    public class Room
    {
        public int beginX;
        public int sizeX;
        public int beginY;
        public int sizeY;
        public bool isConnected;
        public bool visited;
        private System.Random rnd;
        public List<int> connectedRooms;

        public Room(int bx, int sx, int by, int sy)
        {
            visited = false;
            connectedRooms = new List<int>();
            rnd = new System.Random();
            beginX = bx;
            sizeX = sx;
            beginY = by;
            sizeY = sy;
            isConnected = false;
        }
        public Point PointFromWall(int direction)
        {
            Point point;
            point.X = 0;
            point.Y = 0;
            switch (direction)
            {
                case 0:
                    point.X = rnd.Next(beginX + 2, beginX + sizeX - 2);
                    point.Y = beginY;
                    return point;
                case 1:
                    point.X = rnd.Next(beginX + 2, beginX + sizeX - 2);
                    point.Y = beginY + sizeY;
                    return point;
                case 2:
                    point.X = beginX + sizeX;
                    point.Y = rnd.Next(beginY + 2, beginY + sizeY - 2);
                    return point;
                case 3:
                    point.X = beginX;
                    point.Y = rnd.Next(beginY + 2, beginY + sizeY - 2);
                    return point;
            }
            return point;
        }

        public void AddConnection(int roomID)
        {
            connectedRooms.Add(roomID);
        }

        public Point RandomPointFromRoom()
        {
            Point pnt;
            pnt.X = rnd.Next(beginX + 1, beginX + sizeX - 1);
            pnt.Y = rnd.Next(beginY + 1, beginY + sizeY - 1);
            return pnt;
        }
    }

    public struct Point
    {
        public int X;
        public int Y;
    }
}
