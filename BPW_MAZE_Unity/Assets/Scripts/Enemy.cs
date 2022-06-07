using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum State { Patrol, Follow, Attack }
    public State state;

    public float damage;
    public float health;
    public float timeToMove = 0.5f;

    EnemyPlayerDetection sightCollider;

    EnemyCollision upCollider;
    EnemyCollision rightCollider;
    EnemyCollision downCollider;
    EnemyCollision leftCollider;

    Vector3 origPos;
    Vector3 targetPos;
    Vector3 playerPos;

    int infiniteLoopDetection;
    readonly int infiniteDetectionLoopAmount = 50;

    int direction;
    int distanceXFromPlayer;
    int distanceYFromPlayer;

    private PlayerScript playerScript;

    void Start()
    {
        PlayerScript.OnNextTurn += AI;

        playerScript = FindObjectOfType<PlayerScript>();

        sightCollider = gameObject.transform.GetChild(4).gameObject.GetComponent<EnemyPlayerDetection>();

        upCollider = gameObject.transform.GetChild(0).gameObject.GetComponent<EnemyCollision>();
        rightCollider = gameObject.transform.GetChild(1).gameObject.GetComponent<EnemyCollision>();
        downCollider = gameObject.transform.GetChild(2).gameObject.GetComponent<EnemyCollision>();
        leftCollider = gameObject.transform.GetChild(3).gameObject.GetComponent<EnemyCollision>();
    }

    public void Update()
    {
        if (upCollider.collidingWithPlayer || rightCollider.collidingWithPlayer || downCollider.collidingWithPlayer || leftCollider.collidingWithPlayer)
        {
            state = State.Attack;
        }
    }

    public void AI()
    {
        switch (state)
        {
            case State.Patrol:
                Patrol();
                break;

            case State.Follow:
                Follow();
                break;

            case State.Attack:
                Attack();
                break;
        }

        if (sightCollider.detectingPlayer == false)
        {
            state = State.Patrol;
        }

        if (sightCollider.detectingPlayer == true)
        {
            state = State.Follow;
        }
    }

    public void Patrol()
    {
        direction = Random.Range(0, 4);
        Move();
    }

    public void Follow()
    {
        CalcRouteToPLayer();
        Move();
    }

    public void Attack()
    {
        playerScript.playerIsBeeingAttacked = true;
    }

    public void Move()
    {
        infiniteLoopDetection += 1;
        if (infiniteLoopDetection == infiniteDetectionLoopAmount)
        {
            direction = 4;
        }

        //check if 4 sides around the enemy are colliding and move

        //up
        if (direction == 0)
        {
            if (upCollider.collidingWithWall == true)
            {
                if (state.Equals(State.Patrol))
                {
                    Patrol();
                }
                
                if (state.Equals(State.Follow))
                {
                    Follow();
                }
            }
            
            if (upCollider.collidingWithPlayer == true)
            {
                state = State.Attack;
            }

            else
            {
                StartCoroutine(MoveEnemy(new Vector3(0, 1f, 0)));
            }
        }

        //right
        if (direction == 1)
        {
            if (rightCollider.collidingWithWall == true)
            {
                if (state.Equals(State.Patrol))
                {
                    Patrol();
                }

                if (state.Equals(State.Follow))
                {
                    Follow();
                }
            }

            if (rightCollider.collidingWithPlayer == true)
            {
                state = State.Attack;
            }

            else
            {
                StartCoroutine(MoveEnemy(new Vector3(1f, 0, 0)));
            }
        }

        //down
        if (direction == 2)
        {
            if (downCollider.collidingWithWall == true)
            {
                if (state.Equals(State.Patrol))
                {
                    Patrol();
                }

                if (state.Equals(State.Follow))
                {
                    Follow();
                }
            }

            if (downCollider.collidingWithPlayer == true)
            {
                state = State.Attack;
            }

            else
            {
                StartCoroutine(MoveEnemy(new Vector3(0, -1f, 0)));
            }
        }

        //left
        if (direction == 3)
        {
            if (leftCollider.collidingWithWall == true)
            {
                if (state.Equals(State.Patrol))
                {
                    Patrol();
                }

                if (state.Equals(State.Follow))
                {
                    Follow();
                }
            }
            else
            {
                StartCoroutine(MoveEnemy(new Vector3(-1f, 0, 0)));
            }
        }

        if (direction == 4)
        {
            //Debug.Log("EnemyCantMove");
        }
    }

    public void CalcRouteToPLayer()
    {
        playerPos = playerScript.playerLocation;

        distanceXFromPlayer = (int)(playerPos.x - transform.position.x); 
        distanceYFromPlayer = (int)(playerPos.y - transform.position.y); 

        if (distanceYFromPlayer >= 1)
        {
            direction = 0; // Up
        }

        if (distanceYFromPlayer <= -1)
        {
            direction = 2; // Down
        }

        if (distanceXFromPlayer >= 1)
        {
            direction = 1; // Right
        }

        if (distanceXFromPlayer <= -1)
        {
            direction = 3; // Left
        }
    }

    private IEnumerator MoveEnemy(Vector3 direction)
    {
        float elapsedTime = 0;

        infiniteLoopDetection = 0;

        origPos = transform.position;
        targetPos = origPos + direction;

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos;
    }

    public void OnDeath()
    {
        PlayerScript.OnNextTurn -= AI;
    }
}