using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float damage;
    public float health;

    void Start()
    {
        PlayerScript.OnNextTurn += AI;
    }

    public void AI()
    {
        //Senses - 4 sides around the enemy
        //Senses - player detection
    }

    public void OnDeath()
    {
        PlayerScript.OnNextTurn -= AI;
    }
}