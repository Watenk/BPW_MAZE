using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerMainMenu : MonoBehaviour
{
    public void startGame()
    {
        SceneManager.LoadScene("lvl01");
    }
}