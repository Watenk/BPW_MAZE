using MazeGenerator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //FindObject
    private MazeGenerator.MazeGen mazeGenerator;

    private void Awake()
    {
        //FindObject
        mazeGenerator = FindObjectOfType<MazeGenerator.MazeGen>();
    }

    public void SpawnPlayer()
    {
        Vector3Int spawnLocation = mazeGenerator.roomList[0].GetCenter();
        gameObject.transform.position = new Vector3Int (spawnLocation.x, spawnLocation.y, -1);
    }

    void Update()
    {
        
    }
}
