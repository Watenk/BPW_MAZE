using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddEnemy : MonoBehaviour
{
    public void addEnemy(GameObject enemy, Vector3 location, float _damage, float _health)
    {
        Enemy newEnemy = new Enemy();
        newEnemy.enemy = Instantiate(enemy, location, Quaternion.identity);
    }
}

public class Enemy
{
    public GameObject enemy;
    public float damage;
    public float health;

    public Enemy()
    {

    }
}