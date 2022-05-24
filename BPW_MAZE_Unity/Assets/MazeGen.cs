using System.Collections;
using System.Collections.Generic;
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
                int maxZ = minX + Random.Range(minRoomSize, maxRoomSize + 1);

                //New Room
                Room room = new Room(minX, maxX, minZ, minZ);
                if (CanRoomFit(room))
                {
                    AddRoomToMaze(room);
                }
                else
                {
                    i--;
                }
            }
            print("All rooms generated " + roomList.Count + " " + mazeDictionary.Count);
            SpawnMaze();
        }

        public bool CanRoomFit(Room room)
        {
            for (int x = room.minX; x < room.maxX; x++)
            {
                for (int z = room.minZ; z < room.maxZ; z++)
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
                        Instantiate(wall, kv.Key, Quaternion.identity, transform);
                        break;

                    case TileType.Wall:
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
            minZ = _maxZ;
        }
    }
}