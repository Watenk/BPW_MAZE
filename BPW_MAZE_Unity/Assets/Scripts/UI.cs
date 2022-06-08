using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    PlayerScript playerScript;
    GameManager gameManager;
    GameObject attackMenu;

    public Slider healthSlider;
    public TextMeshProUGUI turnsLeft;

    private void Start()
    {
        playerScript = FindObjectOfType<PlayerScript>();
        gameManager = FindObjectOfType<GameManager>();
        attackMenu = gameObject.transform.GetChild(1).gameObject;

        healthSlider.maxValue = playerScript.playerHealth;
    }

    private void Update()
    {
        if (playerScript.playerIsBeeingAttacked == true)
        {
            attackMenu.SetActive(true);
        }
        else
        {
            attackMenu.SetActive(false);
        }

        healthSlider.value = playerScript.playerHealth;
        turnsLeft.text = gameManager.turnsLeft.ToString();
    }
}
