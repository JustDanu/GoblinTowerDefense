using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class World1LevelSelect : MonoBehaviour
{
    public List<InsideList> levelStars;
    public List<Button> levelButtons;
    public GameObject levelDescription;
    public TMP_Text currentLevelText;
    private int currentLevel;
    public void Start()
    {
        UnlockLevels();
    }
    public void LevelSelect(int level)
    {
        currentLevel = level;
        currentLevelText.text = "Level "+(level-2);
        levelDescription.SetActive(true);
    }
    public void SelectLevel()
    {
        SceneManager.LoadScene(currentLevel);
    }
    public void Back()
    {
        levelDescription.SetActive(false);  
    }

    public void BackToWorldSelect()
    {
        SceneManager.LoadScene(1);
    }

    private void UnlockLevels()
    {
        foreach(int level in SaveAndLoadSystem.Instance.playerData.levelsUnlocked)
        {
            levelButtons[level].interactable = true;
            GiveStarsForLevel(level);
        }
    }

    private void GiveStarsForLevel(int level)
    {
        if(SaveAndLoadSystem.Instance.playerData.levelsBeaten.ContainsKey(level))
        {
            int starsEarned = SaveAndLoadSystem.Instance.playerData.levelsBeaten[level].starsEarned;
            for(int i = 0; i < starsEarned; i++)
            {
                levelStars[level].insideList[i].SetActive(true);
            }
        }
        
    }

    [System.Serializable]
    public class InsideList
    {
        public List<GameObject> insideList;

        public InsideList()
        {
            insideList = new List<GameObject>();
        }
    }
}
