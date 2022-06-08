using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    public bool collidingWithWall = false;
    public bool collidingWithPlayer = false;

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            collidingWithWall = true;
        }

        if (other.gameObject.CompareTag("Player"))
        {
            collidingWithPlayer = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            collidingWithWall = false;
        }

        if (other.gameObject.CompareTag("Player"))
        {
            collidingWithPlayer = false;
        }
    }
}
