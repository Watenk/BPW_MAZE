using MazeGenerator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //FindObject
    private MazeGenerator.MazeGen mazeGenerator;

    public bool canMove = false;

    private bool isMoving = false;
    private Vector3 origPos, targetPos;
    private Vector3 up, left, down, right;
    private float timeToMove = 0.2f;

    private void Awake()
    {
        //FindObject
        mazeGenerator = FindObjectOfType<MazeGenerator.MazeGen>();
    }

    void Update()
    {
        if (!isMoving && canMove == true)
        {
            if (Input.GetKey(KeyCode.W))
            {
                StartCoroutine(MovePlayer(up = new Vector3(0, 1f, 0)));
            }

            if (Input.GetKey(KeyCode.A))
            {
                StartCoroutine(MovePlayer(left = new Vector3(-1f, 0, 0)));
            }

            if (Input.GetKey(KeyCode.S))
            {
                StartCoroutine(MovePlayer(down = new Vector3(0, -1f, 0)));
            }

            if (Input.GetKey(KeyCode.D))
            {
                StartCoroutine(MovePlayer(right = new Vector3(1f, 0, 0)));
            }
        }
    }

    private IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;

        float elapsedTime = 0;
        origPos = transform.position;
        targetPos = origPos + direction;

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;

        isMoving = false;
        canMove = false;
    }

    public void SpawnPlayer()
    {
        Vector3Int spawnLocation = mazeGenerator.roomList[0].GetCenter();
        gameObject.transform.position = new Vector3Int(spawnLocation.x, spawnLocation.y, -1);
    }
}
