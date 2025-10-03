using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public bool sounds;
    public bool musics;
    public float volume;
    public float music;
    public Dictionary<int, LevelProgress> levelsBeaten;
    public List<int> levelsUnlocked; 
    public List<InsideList> towersUnlocked;
    public int stars;
    public int starsSpent;

    public PlayerData()
    {
        musics = true;
        sounds = true;

        volume = 0.5f;
        music = 0.5f;
        
        stars = 0;
        starsSpent = 0;
    }
    public void InitializeDefaultValues()
    {
        levelsBeaten = new Dictionary<int, LevelProgress>(); // Level id then level difficulty, and with how many stars
        levelsUnlocked = new List<int>{0};
        towersUnlocked = new List<InsideList>();
        for(int i = 0; i < 5; i++)
        {
            towersUnlocked.Add(new InsideList());
        }
        towersUnlocked[0].insideList.Add(0);
        towersUnlocked[1].insideList.Add(0);
    }
}



[System.Serializable]
public class InsideList
{
    public List<int> insideList;

    public InsideList()
    {
        insideList = new List<int>();
    }
}

[System.Serializable]
public class LevelProgress
{
    public string difficulty;
    public int starsEarned;

    public LevelProgress(string difficulty, int starsEarned)
    {
        this.difficulty = difficulty;
        this.starsEarned = starsEarned;
    }
}