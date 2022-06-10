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

    //HeathBar
    public Slider healthSlider;
    public TextMeshProUGUI healthText;

    //Turns
    public TextMeshProUGUI turnsLeft;

    public GameObject inventory;

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

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (inventory.activeSelf == false)
            {
                inventory.SetActive(true);
            }
            else
            {
                inventory.SetActive(false);
            }
        }

        healthSlider.value = playerScript.playerHealth;
        healthText.text = playerScript.playerHealth.ToString();

        turnsLeft.text = gameManager.turnsLeft.ToString();
    }
}
