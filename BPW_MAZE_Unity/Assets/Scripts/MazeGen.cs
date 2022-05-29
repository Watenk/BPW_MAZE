using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MazeGenerator
{
    public enum TileType { Floor, Wall }

    public class MazeGen : MonoBehaviour
    {
        //Maze Settings:
        //Maze size
        public int gridX = 100;
        public int gridY = 100;
        //Rooms count
        public int rooms = 10;
        //Rooms size
        public int minRoomSize = 3;
        public int maxRoomSize = 7;
        //Enum Gameobjects
        public GameObject floor;
        public GameObject wall;

        //Maze Dictionary
        public Dictionary<Vector3Int, TileType> mazeDictionary = new Dictionary<Vector3Int, TileType>();
        public List<Room> roomList = new List<Room>();

        //FindObject
        private PlayerScript playerScript;

        void Start()
        {
            //FindObject
            playerScript = FindObjectOfType<PlayerScript>();

            Generate();
        }

        //Generate Rooms
        public void Generate()
        {
            for (int i = 0; i < rooms; i++)
            {
                //Random Location
                int minX = Random.Range(0, gridX);
                int maxX = minX + Random.Range(minRoomSize, maxRoomSize + 1);
                int minY = Random.Range(0, gridX);
                int maxY = minY + Random.Range(minRoomSize, maxRoomSize + 1);

                //New Room
                Room room = new Room(minX, maxX, minY, maxY);
                if (CanRoomFit(room))
                {
                    AddRoomToMaze(room);
                }
                else
                {
                    i--;
                }
            }
            //Connect Rooms With Corridors
            for (int i = 0; i < roomList.Count; i++)
            {
                Room roomOne = roomList[i];
                Room roomTwo = roomList[(i + Random.Range(1, roomList.Count)) % roomList.Count];
                ConnectRooms(roomOne, roomTwo);
            }
            Walls();
            SpawnMaze();
            playerScript.SpawnPlayer();
        }

        public bool CanRoomFit(Room room)
        {
            for (int x = room.minX - 1; x < room.maxX + 1; x++)
            {
                for (int y = room.minY - 1; y < room.maxY + 1; y++)
                {
                    if (mazeDictionary.ContainsKey(new Vector3Int(x, y, 0)))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void AddRoomToMaze(Room room)
        {
            for (int x = room.minX; x < room.maxX; x++)
            {
                for (int y = room.minY; y < room.maxY; y++)
                {
                    mazeDictionary.Add(new Vector3Int(x, y, 0), TileType.Floor);
                }
            }
            roomList.Add(room);
        }

        public void ConnectRooms(Room _roomOne, Room _roomTwo)
        {
            Vector3Int posOne = _roomOne.GetCenter();
            Vector3Int posTwo = _roomTwo.GetCenter();

            int x = 0;
            //X-As
            int directionX = posTwo.x > posOne.x ? 1 : -1;

            for (x = posOne.x; x != posTwo.x; x += directionX)
            {
                Vector3Int pos = new Vector3Int(x, posOne.y, 0);
                if (mazeDictionary.ContainsKey(pos)) { continue; }
                mazeDictionary.Add(pos, TileType.Floor);
            }

            //Z-As
            int directionY = posTwo.y > posOne.y ? 1 : -1;

            for (int y = posOne.y; y != posTwo.y; y += directionY)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                if (mazeDictionary.ContainsKey(pos)) { continue; }
                mazeDictionary.Add(pos, TileType.Floor);
            }
        }

        public void Walls()
        {
            var keys = mazeDictionary.Keys.ToList();
            foreach (var kv in keys)
            {
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        if (Mathf.Abs(x) == Mathf.Abs(y)) { continue; }

                        Vector3Int newPos = kv + new Vector3Int(x, y, 0);
                        if (mazeDictionary.ContainsKey(newPos)) { continue; }
                        mazeDictionary.Add(newPos, TileType.Wall);
                    }
                }
            }
        }

        public void SpawnMaze()
        {
            foreach (KeyValuePair<Vector3Int, TileType> kv in mazeDictionary)
            {
                switch (kv.Value)
                {
                    case TileType.Floor:
                        Instantiate(floor, kv.Key, Quaternion.Euler(-90, 0, 0), transform);
                        break;

                    case TileType.Wall:
                        Instantiate(wall, kv.Key, Quaternion.Euler(-90, 0, 0), transform);
                        break;
                }
            }
        }
    }

    public class Room
    {
        public int minX, maxX, minY, maxY;
        public Room(int _minX, int _maxX, int _minY, int _maxY)
        {
            minX = _minX;
            maxX = _maxX;
            minY = _minY;
            maxY = _maxY;
        }

        public Vector3Int GetCenter()
        {
            return new Vector3Int(Mathf.RoundToInt(Mathf.Lerp((float)minX, (float)maxX, 0.5f)), Mathf.RoundToInt(Mathf.Lerp((float)minY, (float)maxY, 0.5f)), 0);
        }
    }
}