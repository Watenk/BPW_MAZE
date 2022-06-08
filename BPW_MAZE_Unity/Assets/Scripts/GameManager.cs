using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int turnsLeft;
    public int score;

    private void Start()
    {
        PlayerScript.OnNextTurn += NextTurn;
    }

    public void NextTurn()
    {
        score += 1;
        turnsLeft -= 1;
    }

    private void Update()
    {
        if (turnsLeft <= 0)
        {
            print("GameOver");
        }
    }
}
