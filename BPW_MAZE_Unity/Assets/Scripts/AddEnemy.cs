using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddEnemy : MonoBehaviour
{
    //RoomEnemy

    public void addRoomEnemy(GameObject enemy, Vector3 location, float _damage, float _health)
    {
        Instantiate(enemy, location, Quaternion.identity);

        damage = _damage;
        health = _health;
    }
}

public class Enemy
{
    public GameObject enemy;
    float damage;
    float health;
}
