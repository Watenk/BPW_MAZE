using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool makeManualMove;

    public float damage;
    public float health;
    public float timeToMove = 0.5f;

    EnemyWallCollision sightCollider;

    EnemyWallCollision upCollider;
    EnemyWallCollision rightCollider;
    EnemyWallCollision downCollider;
    EnemyWallCollision leftCollider;

    Vector3 origPos;
    Vector3 targetPos;

    void Start()
    {
        PlayerScript.OnNextTurn += AI;

        upCollider = gameObject.transform.GetChild(0).gameObject.GetComponent<EnemyWallCollision>();
        rightCollider = gameObject.transform.GetChild(1).gameObject.GetComponent<EnemyWallCollision>();
        downCollider = gameObject.transform.GetChild(2).gameObject.GetComponent<EnemyWallCollision>();
        leftCollider = gameObject.transform.GetChild(3).gameObject.GetComponent<EnemyWallCollision>();

        sightCollider = gameObject.transform.GetChild(4).gameObject.GetComponent<EnemyWallCollision>();
    }

    public void Update()
    {
        if (makeManualMove == true)
        {
            AI();
        }
    }

    public void AI()
    {
        makeManualMove = false;
    ifEnemyIsColliding:

        //RandomDirection
        int direction = Random.Range(0, 4);

        //check if 4 sides around the enemy are colliding
        if (direction == 0)
        {
            //up
            if (upCollider.enemyColliding == true)
            {
                goto ifEnemyIsColliding;
            }
            else
            {
                StartCoroutine(MoveEnemy(new Vector3(0, 1f, 0)));
            }

        }

        if (direction == 1)
        {
            //right
            if (rightCollider.enemyColliding == true)
            {
                goto ifEnemyIsColliding;
            }
            else
            {
                StartCoroutine(MoveEnemy(new Vector3(1f, 0, 0)));
            }
        }

        if (direction == 2)
        {
            //down
            if (downCollider.enemyColliding == true)
            {
                goto ifEnemyIsColliding;
            }
            else
            {
                StartCoroutine(MoveEnemy(new Vector3(0, -1f, 0)));
            }
        }

        if (direction == 3)
        {
            //left
            if (leftCollider.enemyColliding == true)
            {
                goto ifEnemyIsColliding;
            }
            else
            {
                StartCoroutine(MoveEnemy(new Vector3(-1f, 0, 0)));
            }
        }
    }

    private IEnumerator MoveEnemy(Vector3 direction)
    {
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
    }

    public void OnDeath()
    {
        PlayerScript.OnNextTurn -= AI;
    }
}