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
        public int gridWidth = 100;
        public int gridHeight = 100;
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

        void Start()
        {
            Generate();
        }

        //Generate Rooms
        public void Generate()
        {
            for (int i = 0; i < rooms; i++)
            {
                //Random Location
                int minX = Random.Range(0, gridWidth);
                int maxX = minX + Random.Range(minRoomSize, maxRoomSize + 1);
                int minZ = Random.Range(0, gridWidth);
                int maxZ = minZ + Random.Range(minRoomSize, maxRoomSize + 1);

                //New Room
                Room room = new Room(minX, maxX, minZ, maxZ);
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
        }

        public void Walls()
        {
            var keys = mazeDictionary.Keys.ToList();
            foreach(var kv in keys)
            {
                for (int x = -1; x <= 1; x++)
                {
                    for (int z = -1; z <= 1; z++)
                    {
                        if(Mathf.Abs(x) == Mathf.Abs(z)) { continue; }

                        Vector3Int newPos = kv + new Vector3Int(x, 0, z);
                        if(mazeDictionary.ContainsKey(newPos)) { continue; }
                        mazeDictionary.Add(newPos, TileType.Wall);
                    }
                }
            }
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
                Vector3Int pos = new Vector3Int(x, 0, posOne.z);
                if (mazeDictionary.ContainsKey(pos)) { continue; }
                mazeDictionary.Add(pos, TileType.Floor);
            }

            //Z-As
            int directionZ = posTwo.z > posOne.z ? 1 : -1;

            for (int z = posOne.z; z != posTwo.z; z += directionZ)
            {
                Vector3Int pos = new Vector3Int(x, 0, z);
                if (mazeDictionary.ContainsKey(pos)) { continue; }
                mazeDictionary.Add(pos, TileType.Floor);
            }
        }

        public bool CanRoomFit(Room room)
        {
            for (int x = room.minX-1; x < room.maxX+1; x++)
            {
                for (int z = room.minZ-1; z < room.maxZ+1; z++)
                {
                    if (mazeDictionary.ContainsKey(new Vector3Int(x, 0, z)))
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
                for (int z = room.minZ; z < room.maxZ; z++)
                {
                    mazeDictionary.Add(new Vector3Int(x, 0, z), TileType.Floor);
                }
            }
            roomList.Add(room);
        }

        public void SpawnMaze()
        {
            foreach (KeyValuePair<Vector3Int, TileType> kv in mazeDictionary)
            {
                switch (kv.Value)
                {
                    case TileType.Floor:
                        Instantiate(floor, kv.Key, Quaternion.identity, transform);
                        break;

                    case TileType.Wall:
                        Instantiate(wall, kv.Key, Quaternion.identity, transform);
                        break;
                }
            }
        }
    }

    public class Room
    {
        public int minX, maxX, minZ, maxZ;
        public Room(int _minX, int _maxX, int _minZ, int _maxZ)
        {
            minX = _minX;
            maxX = _maxX;
            minZ = _minZ;
            maxZ = _maxZ;
        }

        public Vector3Int GetCenter()
        {
            return new Vector3Int(Mathf.RoundToInt(Mathf.Lerp((float)minX, (float)maxX, 0.5f)), 0, Mathf.RoundToInt(Mathf.Lerp((float)minZ, (float)maxZ, 0.5f)));
        }
    }
}