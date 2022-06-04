using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWallCollision : MonoBehaviour
{
    public bool enemyColliding = false;

    public void OnTriggerStay(Collider other)
    {
        enemyColliding = true;
    }

    public void OnTriggerExit(Collider other)
    {
        enemyColliding = false;
    }


}
