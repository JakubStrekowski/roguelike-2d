using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using System.Diagnostics;

using static AStarCorridors;

public class DungeonStruct
{
    public int[][] dungeon;
    public int[][] characterMap;
    public int[][] itemMap;

    public DungeonStruct(int sizeX, int sizeY)
    {
        dungeon = new int[sizeY][];
        characterMap = new int[sizeY][];
        itemMap = new int[sizeY][];
        for (int i = 0; i < sizeY; i++)
        {
            dungeon[i] = new int[sizeX];
            characterMap[i] = new int[sizeX];
            itemMap[i] = new int[sizeX];
            for (int j = 0; j < sizeX; j++)
            {
                dungeon[i][j] = 10;
                characterMap[i][j] = 0;
                itemMap[i][j] = 0;
            }
        }
    }
}



public class LevelGenerator
{
    private DungeonStruct dungeonStructure;
    private List<Room> rooms;
    private List<Room> connectedRooms;
    private int mapSizeX;
    private int mapSizeY;
    System.Random rnd;

    public LevelGenerator(int sizeX, int sizeY)
    {
        mapSizeX = sizeX * 12;
        mapSizeY = sizeY * 12;
        connectedRooms = new List<Room>();
        rooms = new List<Room>();
        rnd = new System.Random();
        dungeonStructure = new DungeonStruct(sizeX, sizeY);
    }

    public DungeonStruct CreateDungeon(int sizeX, int sizeY, int roomsAmnt)
    {
        UnityEngine.Debug.Log("Creting empty sturcture:");
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();
        mapSizeX = sizeX * 12;
        mapSizeY = sizeY * 12;
        rooms = new List<Room>();
        rnd = new System.Random();
        dungeonStructure = new DungeonStruct(mapSizeX, mapSizeY);

        DungeonGrid dungeonGrid = new DungeonGrid(dungeonStructure.dungeon);

        int roomSizeX = rnd.Next(4, 8);
        int roomSizeY = rnd.Next(4, 8);
        int direction = 0;

        UnityEngine.Debug.Log("Creting empty sturcture finished in" + stopWatch.ElapsedMilliseconds);
        stopWatch.Reset();
        stopWatch.Start();
        UnityEngine.Debug.Log("Creating rooms");

        Point roomBeginCoordinates = dungeonGrid.GetRoomCreationPoint(direction, roomSizeX, roomSizeY);
        CreateIfPossible(roomSizeX, roomSizeY, direction, roomBeginCoordinates.X, roomBeginCoordinates.Y, false);
        rooms[0].isConnected = true;
        connectedRooms.Add(rooms[0]);
        int selectedRoom;
        while (rooms.Count != roomsAmnt)
        {

            selectedRoom = rnd.Next(rooms.Count);

            direction = rnd.Next(4);

            Point nextPlace = dungeonGrid.GetRoomCreationPoint(direction, roomSizeX, roomSizeY);
            CreateIfPossible(rnd.Next(6, 12), rnd.Next(6, 12), direction, nextPlace.X, nextPlace.Y, false);
        }
        int id = 0;

        UnityEngine.Debug.Log("Rooms created in " + stopWatch.ElapsedTicks);
        stopWatch.Reset();
        stopWatch.Start();
        UnityEngine.Debug.Log("Connecting rooms");

        AStarCorridors aStarCorridors = new AStarCorridors(mapSizeX, mapSizeY, dungeonStructure.dungeon, rooms);
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
                List<Node>path= aStarCorridors.ConnectTwoRoomsNoCollisions(dungeonStructure.dungeon, rooms, room, rndroom);
                if(!(path is null))
                {
                    foreach (Node node in path)
                    {
                        if (dungeonStructure.dungeon[node.position.Y][node.position.X] > 9) dungeonStructure.dungeon[node.position.Y][node.position.X] = 0;
                    }
                    room.AddConnection(rand);
                    rndroom.AddConnection(id);
                    room.isConnected = true;
                    rndroom.isConnected = true;
                }
                else
                {
                    UnityEngine.Debug.Log("Path was null");
                }
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
            List<Node>path= aStarCorridors.ConnectTwoRoomsNoCollisions(dungeonStructure.dungeon, rooms, connectedRooms[room1], connectedRooms[room2]);
                if(!(path is null))
                {
                    foreach (Node node in path)
                    {
                        if (dungeonStructure.dungeon[node.position.Y][node.position.X] > 9) dungeonStructure.dungeon[node.position.Y][node.position.X] = 0;
                    }
                    connectedRooms[room1].AddConnection(room2);
                    connectedRooms[room2].AddConnection(room1);
                    connectedRooms[room1].isConnected = true;
                    connectedRooms[room2].isConnected = true;
                }
                else
                {
                     UnityEngine.Debug.Log("Path was null");
                }
        }

        UnityEngine.Debug.Log("Rooms connected in " + stopWatch.ElapsedTicks);
        stopWatch.Reset();
        stopWatch.Start();
        UnityEngine.Debug.Log("Placing objects");

        //placing enemies and items
        for (int i = 1; i < rooms.Count; i++)
        {
            int enemyAmnt = rnd.Next(2, 6);
            int goldChance = rnd.Next(3);
            int healthPotsChance = rnd.Next(10);
            for (int j = 0; j < enemyAmnt; j++)
            {
                Point pnt = rooms[i].RandomPointFromRoom();
                dungeonStructure.characterMap[pnt.Y][pnt.X] = Random.Range(2, 6);
            }
            if (goldChance == 2)
            {
                Point pnt = rooms[i].RandomPointFromRoom();
                dungeonStructure.itemMap[pnt.Y][pnt.X] = 1;
            }
            if (healthPotsChance == 9)
            {
                Point pnt = rooms[i].RandomPointFromRoom();
                dungeonStructure.itemMap[pnt.Y][pnt.X] = 2;
            }
        }

        Room lastRoom = FindFurthest(rooms[0]);
        Point randInLast = lastRoom.RandomPointFromRoom();
        dungeonStructure.dungeon[randInLast.Y][randInLast.X] = 20;
        PutHero();

        UnityEngine.Debug.Log("Obcjects placed in " + stopWatch.ElapsedTicks);
        stopWatch.Stop();

        /*
        foreach (int[] itgr in dungeonStructure.characterMap)
        {
            string s = "";
            foreach (int integer in itgr)
            {
                s += integer.ToString()+" ";
            }
            //Debug.Log(s);
        }
        */
        return dungeonStructure;

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
        /*
        switch (direction) //setting room begin points depending on set direction 0-up, 1-down, 2-right, 3-left
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
        */
        if (!(beginX >= 0) || !(beginY >= 0) || sizeX + beginX > mapSizeX || sizeY + beginY > mapSizeY) //if room passes borders of map, return failure
        {
            return false;
        }
        for (int i = beginY; i < sizeY + beginY; i++)
        {
            for (int j = beginX; j < sizeX + beginX; j++)
            {
                if (dungeonStructure.dungeon[i][j] != 10) //room already exists here, return failure
                {
                    return false;
                }
            }
        }
        Room room = new Room(beginX, sizeX, beginY, sizeY);  //saving room parameters, setting room walls and floors variant
        if (!corridor) rooms.Add(room);
        for (int i = beginY; i < sizeY + beginY; i++)
        {
            for (int j = beginX; j < sizeX + beginX; j++)
            {
                if (!corridor && (i == beginY || j == beginX || i == beginY + sizeY - 1 || j == beginX + sizeX - 1))
                {
                    dungeonStructure.dungeon[i][j] = room.wallVariant; //painting wall
                }
                else
                {
                    dungeonStructure.dungeon[i][j] = room.floorVariant; //painting floor
                }
            }
        }
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
                    if(dungeonStructure.dungeon[possibleRoute.Y][possibleRoute.X]>9)dungeonStructure.dungeon[possibleRoute.Y][possibleRoute.X] = 0;
                    currentPoint = possibleRoute;
                    madeStep = true;
                }
                else
                {
                    possibleRoute.Y += 2;
                    if (MeasureDistance(pointRoom1, possibleRoute).Y < distanceY && !madeStep)
                    {
                        distanceY = MeasureDistance(pointRoom1, possibleRoute).Y;
                        if (dungeonStructure.dungeon[possibleRoute.Y][possibleRoute.X] > 9) dungeonStructure.dungeon[possibleRoute.Y][possibleRoute.X] = 0;
                        currentPoint = possibleRoute;
                        madeStep = true;
                    }
                }
                possibleRoute.Y -= 1;
                possibleRoute.X -= 1;
                if (MeasureDistance(pointRoom1, possibleRoute).X < distanceX && !madeStep)
                {
                    distanceX = MeasureDistance(pointRoom1, possibleRoute).X;
                    if (dungeonStructure.dungeon[possibleRoute.Y][possibleRoute.X] > 9) dungeonStructure.dungeon[possibleRoute.Y][possibleRoute.X] = 0;
                    currentPoint = possibleRoute;
                    madeStep = true;
                }
                else
                {
                    possibleRoute.X += 2;
                    if (MeasureDistance(pointRoom1, possibleRoute).X < distanceX && !madeStep)
                    {
                        distanceX = MeasureDistance(pointRoom1, possibleRoute).X;
                        if (dungeonStructure.dungeon[possibleRoute.Y][possibleRoute.X] > 9) dungeonStructure.dungeon[possibleRoute.Y][possibleRoute.X] = 0;
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
                    if (dungeonStructure.dungeon[possibleRoute.Y][possibleRoute.X] > 9) dungeonStructure.dungeon[possibleRoute.Y][possibleRoute.X] = 0;
                    currentPoint = possibleRoute;
                    madeStep = true;
                }
                else
                {
                    possibleRoute.X += 2;
                    if (MeasureDistance(pointRoom1, possibleRoute).X < distanceX && !madeStep)
                    {
                        distanceX = MeasureDistance(pointRoom1, possibleRoute).X;
                        if (dungeonStructure.dungeon[possibleRoute.Y][possibleRoute.X] > 9) dungeonStructure.dungeon[possibleRoute.Y][possibleRoute.X] = 0;
                        currentPoint = possibleRoute;
                        madeStep = true;
                    }
                }
                possibleRoute.X -= 1;
                possibleRoute.Y -= 1;
                if (MeasureDistance(pointRoom1, possibleRoute).Y < distanceY && !madeStep)
                {
                    distanceY = MeasureDistance(pointRoom1, possibleRoute).Y;
                    if (dungeonStructure.dungeon[possibleRoute.Y][possibleRoute.X] > 9) dungeonStructure.dungeon[possibleRoute.Y][possibleRoute.X] = 0;
                    currentPoint = possibleRoute;
                    madeStep = true;
                }
                else
                {
                    possibleRoute.Y += 2;
                    if (MeasureDistance(pointRoom1, possibleRoute).Y < distanceY && !madeStep)
                    {
                        distanceY = MeasureDistance(pointRoom1, possibleRoute).Y;
                        if (dungeonStructure.dungeon[possibleRoute.Y][possibleRoute.X] > 9) dungeonStructure.dungeon[possibleRoute.Y][possibleRoute.X] = 0;
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


    public static Point MeasureDistance(Point a, Point b)
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
        dungeonStructure.characterMap[posY][posX] = 1;
    }

    public class Room
    {
        public int beginX;
        public int sizeX;
        public int beginY;
        public int sizeY;
        public bool isConnected;
        public bool visited;
        public int floorVariant;
        public int wallVariant;
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
            floorVariant = Random.Range(0, 6);
            wallVariant = Random.Range(10, 13);
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

        public void ChangeFloorStyle()
        {

        }
    }

    public struct Point
    {
        public int X;
        public int Y;
    }

    public class DungeonGridElement
    {
        public int startX;
        public int startY;
        public bool hasRoom;

        public DungeonGridElement(int y, int x)
        {
            startY = y;
            startX = x;
            hasRoom = false;
        }
    }

    public class DungeonGrid
    {
        public DungeonGridElement[][] grid;
        private Collection<DungeonGridElement> emptyGridElements;

        public DungeonGrid(int[][] dungeon)
        {
            //TODO add ceiling to division by 12
            emptyGridElements = new Collection<DungeonGridElement>();
            grid = new DungeonGridElement[dungeon.Length / 12][];
            for (int i = 0; i < grid.Length; i++)
            {
                grid[i] = new DungeonGridElement[dungeon[0].Length / 12];
                for (int j = 0; j < grid[0].Length; j++)
                {
                    grid[i][j] = new DungeonGridElement(i * 12, j * 12);
                    if (i == grid.Length || j == grid[0].Length)
                    {
                        if (!(dungeon.Length - grid[i][j].startY < 8 || dungeon[0].Length - grid[i][j].startX < 8))
                        {
                            emptyGridElements.Add(grid[i][j]);
                        }
                    }
                    else
                    {
                        emptyGridElements.Add(grid[i][j]);
                    }
                }
            }
        }

        public Point GetRoomCreationPoint(int direction, int sizeX, int sizeY)
        {
            int selectedGridElem = Random.Range(0, emptyGridElements.Count);
            Point selectedPoint = new Point();
            selectedPoint.X = emptyGridElements[selectedGridElem].startX;
            selectedPoint.Y = emptyGridElements[selectedGridElem].startY;

            //selecting minimal down-left corner depending on direction
            switch (direction)
            {
                case 0:
                    selectedPoint.X += (sizeX / 2);
                    break;
                case 1:
                    selectedPoint.Y += sizeY;
                    selectedPoint.X += (sizeX / 2);
                    break;
                case 2:
                    selectedPoint.Y += (sizeY/2);
                    break;
                case 3:
                    selectedPoint.Y += (sizeY/2);
                    selectedPoint.X += sizeX;
                    break;
            }

            //adding random values to coordinates
            selectedPoint.X += Random.Range(0, (6 - (sizeX)));
            selectedPoint.Y += Random.Range(0, (6 - (sizeY)));

            emptyGridElements.RemoveAt(selectedGridElem);

            return selectedPoint;
        }
    }
}
