using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class SaveAndLoadSystem : MonoBehaviour
{
    public static SaveAndLoadSystem Instance;
    public PlayerData playerData;
    private string filePath;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // So its the same gameobject across scenes
            filePath = Application.persistentDataPath + "/save.json";
            LoadPlayerData();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void LoadPlayerData()
    {
        if(File.Exists(filePath))
        {
            Debug.Log($"Save file path: {Application.persistentDataPath}/save.json");
            string json = File.ReadAllText(filePath);
            playerData = JsonConvert.DeserializeObject<PlayerData>(json);
        }
        else
        {
            Debug.LogWarning("Save File not Found. Going to default data.");
            playerData = new PlayerData(); // Create new file with Default values
            playerData.InitializeDefaultValues();
        }
    }

    public void SavePlayerData()
    {
        string json = JsonConvert.SerializeObject(playerData); //Convert player data to a json file
        File.WriteAllText(filePath, json);
    }
}
