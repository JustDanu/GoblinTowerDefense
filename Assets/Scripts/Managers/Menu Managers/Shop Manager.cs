using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;

public class ShopManager : MonoBehaviour
{
    private List<Button> shopButtons;
    public TMP_Text stars;

    public void Start()
    {
        UpdateStars();
        shopButtons = new List<Button>(FindObjectsOfType<Button>());
        //Not sure how this line works but it works!
        shopButtons.RemoveAll(button => button.gameObject.tag != "Purchase Button");

        TurnOffUnlockedTowers();
    }

    public void BuyTower(int cost)
    {
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
        int towerIDIndex = SeperateNumberAtPosition(0, clickedButton.name);
        int towerID = SeperateNumberAtPosition(1, clickedButton.name);

        if(SaveAndLoadSystem.Instance.playerData.stars >= cost)
        {
            SaveAndLoadSystem.Instance.playerData.starsSpent += cost;
            SaveAndLoadSystem.Instance.playerData.stars -= cost;
            SaveAndLoadSystem.Instance.playerData.towersUnlocked[towerIDIndex].insideList.Add(towerID);
            
            clickedButton.GetComponent<Image>().enabled = false;
            clickedButton.GetComponent<Button>().interactable = false;
            UpdateStars();
            SaveAndLoadSystem.Instance.SavePlayerData();
        }
    }

    public int SeperateNumberAtPosition(int index, string idString)
    {
        string[] splitIDString = idString.Split(",");
        int[] numbers = Array.ConvertAll(splitIDString, int.Parse);

        return numbers[index];
    }

    public void BackToWorldSelect()
    {
        SceneManager.LoadScene(1);
    }

    private void UpdateStars()
    {
        stars.text = SaveAndLoadSystem.Instance.playerData.stars.ToString();
    }

    public void Reset()
    {
        if(SaveAndLoadSystem.Instance.playerData.starsSpent > 0)
        {
            SaveAndLoadSystem.Instance.playerData.stars += SaveAndLoadSystem.Instance.playerData.starsSpent;
            SaveAndLoadSystem.Instance.playerData.starsSpent = 0;
            SaveAndLoadSystem.Instance.playerData.towersUnlocked.Clear();
            for(int i = 0; i < 4; i ++)
            {
                SaveAndLoadSystem.Instance.playerData.towersUnlocked.Add(new InsideList());
            }
            SaveAndLoadSystem.Instance.playerData.towersUnlocked[0].insideList.Add(0);
            SaveAndLoadSystem.Instance.playerData.towersUnlocked[1].insideList.Add(0);
            UpdateStars();
            TurnOnButtons();
        }
    }

    public void TurnOffUnlockedTowers()
    {
        foreach(Button button in shopButtons)
        {
            int towerIDIndex = SeperateNumberAtPosition(0, button.name);
            int towerID = SeperateNumberAtPosition(1, button.name);
            if(SaveAndLoadSystem.Instance.playerData.towersUnlocked.Count > towerIDIndex && SaveAndLoadSystem.Instance.playerData.towersUnlocked[towerIDIndex].insideList.Contains(towerID))
            {
                button.GetComponent<Image>().enabled = false;
                button.interactable = false;
            }
        }
    }
    public void TurnOnButtons()
    {
        foreach(Button button in shopButtons)
        {
            int towerIDIndex = SeperateNumberAtPosition(0, button.name);
            int towerID = SeperateNumberAtPosition(1, button.name);
            if(SaveAndLoadSystem.Instance.playerData.towersUnlocked.Count > towerIDIndex && !SaveAndLoadSystem.Instance.playerData.towersUnlocked[towerIDIndex].insideList.Contains(towerID))
            {
                button.GetComponent<Image>().enabled = true;
                button.interactable = true;
            }
        }
    }


}
