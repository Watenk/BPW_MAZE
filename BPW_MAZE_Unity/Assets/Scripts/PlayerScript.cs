using MazeGenerator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //FindObject
    private MazeGenerator.MazeGen mazeGenerator;
    private FindFlashlight flashlight;

    //PLayerMovement
    public float timeToMove = 0.5f;

    private bool isMoving = false;
    private Vector3 origPos, targetPos;

    //FlashlightMovement
    private bool flashlightIsMoving = false;
    private Vector3 currentRotation = new Vector3 (0, 90, 90);
    private Vector3 rightRotation = new Vector3(0, 90, 90), downRotation = new Vector3(90, 90, 90), LeftRotation = new Vector3(180, 90, 90), upRotation = new Vector3(270, 90, 90);

    //NextTurn Event
    public delegate void NextTurn();
    public static event NextTurn OnNextTurn;

    private void Awake()
    {
        //FindObject
        mazeGenerator = FindObjectOfType<MazeGenerator.MazeGen>();
        flashlight = FindObjectOfType<FindFlashlight>();
    }

    void Update()
    {
        if (!isMoving)
        {
            //Player Movement
            if (Input.GetKey(KeyCode.W))
            {
                StartCoroutine(MovePlayer(new Vector3(0, 1f, 0)));
            }

            if (Input.GetKey(KeyCode.A))
            {
                StartCoroutine(MovePlayer(new Vector3(-1f, 0, 0)));
            }

            if (Input.GetKey(KeyCode.S))
            {
                StartCoroutine(MovePlayer(new Vector3(0, -1f, 0)));
            }

            if (Input.GetKey(KeyCode.D))
            {
                StartCoroutine(MovePlayer(new Vector3(1f, 0, 0)));
            }
        }

        if (!flashlightIsMoving)
        {
            //Player FlashLight
            if (Input.GetKey(KeyCode.RightArrow))
            {
                StartCoroutine(MoveFlashLight(rightRotation));
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                StartCoroutine(MoveFlashLight(downRotation));
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                StartCoroutine(MoveFlashLight(LeftRotation));
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                StartCoroutine(MoveFlashLight(upRotation));
            }
        }
    }

    //MovePlayer
    private IEnumerator MovePlayer(Vector3 direction)
    {
        //Start Moving
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

        OnNextTurn?.Invoke();

        isMoving = false;
    }

    //MoveFlashlight
    private IEnumerator MoveFlashLight(Vector3 targetRotation)
    {
        float elapsedTime = 0;

        flashlightIsMoving = true;

        while (elapsedTime < timeToMove)
        {
            flashlight.transform.rotation = Quaternion.Euler(Vector3.Lerp(currentRotation, targetRotation, (elapsedTime / timeToMove)));
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        flashlight.transform.rotation = Quaternion.Euler(targetRotation.x, 90, 90);
        currentRotation = new Vector3(targetRotation.x, 90, 90);

        flashlightIsMoving = false;
    }

    public void SpawnPlayer()
    {
        Vector3Int spawnLocation = mazeGenerator.roomList[0].GetCenter();
        gameObject.transform.position = new Vector3Int(spawnLocation.x, spawnLocation.y, -1);
    }

    public void OnTriggerEnter(Collider other)
    {
        targetPos = origPos;
    }
}