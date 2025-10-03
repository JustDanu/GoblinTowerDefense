using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;

    public int startingMoney = 100;
    public int startingLives = 10;
    public int currentLives;
    private int currentMoney;

    public TMP_Text moneyText;
    public TMP_Text livesText;

    private void Awake()
    {
        // Ensure there's only one instance of the MoneyManager
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Start()
    {
        currentMoney = startingMoney;
        currentLives = startingLives;
        UpdateLivesUI();
        UpdateMoneyUI();
    }

    public void MoneyEarned(int amount)
    {
        currentMoney += amount;
        UpdateMoneyUI();
    }

    public bool MoneySpent(int amount)
    {
        if(currentMoney >= amount)
        {
            currentMoney -= amount;
            UpdateMoneyUI();
            return true;
        }
        else
        {
            Debug.Log("Not Enough Money");
            return false;
        }
    }

    public bool livesLost(int amount)
    {
        if(currentLives > amount)
        {
            currentLives -= amount;
            UpdateLivesUI();
            return true;
        }
        else
        {
            Debug.Log("you dead");
            return false;
        }
    }

    private void UpdateMoneyUI()
    {
        if(moneyText != null)
        {
            moneyText.text = "$" + currentMoney.ToString();
        }
    }

    private void UpdateLivesUI()
    {
        if(moneyText != null)
        {
            livesText.text = currentLives.ToString();
        }
    }
}
