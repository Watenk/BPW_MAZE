using MazeGenerator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public enum State { Wandering, Attacking }
    public State state;

    public int playerHealth = 50;
    public int damage = 5;

    //FindObject
    private MazeGenerator.MazeGen mazeGenerator;
    private FindFlashlight flashlight;
    private AttackCollider attackCollider;

    //Attack
    public bool playerIsBeeingAttacked = false;

    //PLayerMovement
    public Vector3 playerLocation;
    public float timeToMove = 0.5f;
    public float timeBetweenMove = 0.51f;
    float moveTimer;

    public bool canMove = false;
    private Vector3 origPos, targetPos;

    //FlashlightMovement
    private bool flashlightIsMoving = false;
    private Vector3 currentRotation = new Vector3(0, 90, 90);
    private Vector3 rightRotation = new Vector3(0, 90, 90), downRotation = new Vector3(90, 90, 90), LeftRotation = new Vector3(180, 90, 90), upRotation = new Vector3(270, 90, 90);

    //NextTurn Event
    public delegate void NextTurn();
    public static event NextTurn OnNextTurn;

    private void Awake()
    {
        //FindObject
        mazeGenerator = FindObjectOfType<MazeGenerator.MazeGen>();
        flashlight = FindObjectOfType<FindFlashlight>();
        attackCollider = FindObjectOfType<AttackCollider>();
    }

    void Update()
    {
        switch(state)
        {
            case State.Wandering:
                Wandering();
                break;

            case State.Attacking:
                break;
        }

        playerLocation = transform.position;

        Timers();
        FlashLight();

        if (playerIsBeeingAttacked == true)
        {
            state = State.Attacking;
        }

        if (playerIsBeeingAttacked == false)
        {
            state = State.Wandering;
        }
    }

    public void Wandering()
    {
        PlayerMovement();
    }

    public void SwordAttack()
    {
        attackCollider.targetEnemy.health -= damage;
        OnNextTurn?.Invoke();
    }

    public void PlayerMovement()
    {
        //Player Input
        if (canMove)
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
    }
    
    public void FlashLight()
    {
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

    public void Timers()
    {
        //Move Timer
        if (moveTimer >= 0)
        {
            moveTimer -= Time.deltaTime;
        }
        if (moveTimer <= 0 && playerIsBeeingAttacked == false)
        {
            canMove = true;
        }
    }
    
    private IEnumerator MovePlayer(Vector3 direction)
    {
        float elapsedTime = 0;

        canMove = false;
        moveTimer = timeBetweenMove;

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
        if (other.gameObject.CompareTag("Wall"))
        {
            targetPos = origPos;
        }
    }
}