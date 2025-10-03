using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using System.Linq;
using Unity.VisualScripting;
using System;

public class UiManager : MonoBehaviour
{

    public static UiManager instance;

    [Header("Audio clips")]
    public AudioClip TowerUpgradeFX;

    // Variables for the tower placing menu
    [Header("Tower Placing Menu")]
    public GameObject placeMenu;
    public RectTransform placeMenuTransform; 
    public List<Button> placeButtons;

    public List<TMP_Text> placeMenuButtonsText;

    // Variables for the upgrade menu
    [Header("Tower Upgrade Menu")]
    public GameObject upGradeMenu;
    public List<Button> UpgradeButtons;
    public RectTransform upGradeMenuTransform;
    public TMP_Text towerToUpgradeText;

    //Tower info
    [Header("Tower Info Object")]
    public GameObject towerInfo;

    //Range Indicator info
    [Header("Tower Range Indicator")]
    public GameObject rangeImage;
    public RectTransform rangeIndicator;

    //Range Indicator info
    [Header("Tower Place Indicator")]
    public GameObject placerIcon;
    public RectTransform placerLocation;
    private Image placerImage;

    private bool iconFollow;

    [Header("Tower Access")]
    // The current tower the player clicken on
    public Tower currentTower;
    public Vector2 currentTowerPos;
    // Tower placer access
    public TowerPlacer towerPlacer;
    // Button and text variables

    [Header("Game Over Screen")]
    public GameObject gameOverScreen;

    [Header("Game Won Screen")]
    public GameObject gameWonScreen;
    public TMP_Text starsEarned;

    [Header("Game Paused Screen")]
    public GameObject gamePausedScreen;
    public GameObject gameOptionsScreen;
    private bool isPaused;

    [Header("Voume Sliders")]

    public Slider volumeSlider;
    public Slider musicSlider;

    public List<TMP_Text> upgradeTexts;
    private List<TMP_Text> towerInfoTexts;
    private TMP_Text towerNameText;

    private List<int> buttonPressedAmounts = new List<int>();
    private List<int> upgradeButtonPressedAmounts = new List<int>();

    private int towerAboutToPlace;

    public Tilemap groundTiles;
    public List<Tile> buildableTiles;
    public List<Tile> waterPlacableTiles;

    public bool lastWave;
    public int currentLevelID;
   
    private void Awake()
    {
        // Ensure there's only one instance of the UiManager
        if(instance == null)
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
        SoundFXManager.Instance.StopMusic();
        SoundFXManager.Instance.PlayLevelMusic();
        //Makes sure level runs properly after leaving and coming back.
        Time.timeScale = 1f;
        isPaused = false;
        lastWave = false;

        // Does the necessery volume initializors at the start of every level.
        InitialVolumeStartup();

        placerImage = placerIcon.GetComponent<Image>();

        if(upGradeMenu != null)
        {
            placeButtons = new List<Button>(placeMenu.GetComponentsInChildren<Button>());

            ResetPlaceButtons();

            UpgradeButtons = new List<Button>(upGradeMenu.GetComponentsInChildren<Button>());

            placeMenuButtonsText = new List<TMP_Text>(placeMenu.GetComponentsInChildren<TMP_Text>());

            towerInfoTexts = new List<TMP_Text>(towerInfo.GetComponentsInChildren<TMP_Text>());


            foreach(TMP_Text text in towerInfoTexts)
            {
                if(text.name.Equals("Tower Name")) towerNameText = text;
            }

            for(int i = 0; i < placeMenuButtonsText.Count; i++)
            {
                buttonPressedAmounts.Add(0);
            }
            for(int i = 0; i < upgradeTexts.Count; i++)
            {
                upgradeButtonPressedAmounts.Add(0);
            }
            foreach(Button button in UpgradeButtons)
            {
                button.interactable = false;
            }
        }
        
    }

    public void ResetPlaceButtons()
    {
        for(int i = 0; i < SaveAndLoadSystem.Instance.playerData.towersUnlocked.Count; i++)
        {
            if(SaveAndLoadSystem.Instance.playerData.towersUnlocked[i].insideList.Contains(0))
            {
                placeButtons[i].interactable = true;
            }
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    
    public void LevelSelect(int level)
    {
        SceneManager.LoadScene(level);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene {scene.name} loaded");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        Time.timeScale = 0;
    }
    
    public void UpdateStarText(int amount)
    {
        starsEarned.text = "You Got "+amount+" Stars";
    }

    private void InitialVolumeStartup()
    {
        // Volume sliders are set to the saved values in the file
        volumeSlider.value = SaveAndLoadSystem.Instance.playerData.volume;
        musicSlider.value = SaveAndLoadSystem.Instance.playerData.music;
        SoundFXManager.Instance.UpdateVolumes();

        // Add Listeners for all the sliders so they can change the values in the save system.
        volumeSlider.onValueChanged.AddListener(UpdateVolume);
        musicSlider.onValueChanged.AddListener(UpdateMusic);
        SoundFXManager.Instance.UpdateVolumes();
    }

    public void UpdateVolume(float value)
    {
        SaveAndLoadSystem.Instance.playerData.volume = value;
    }
    public void UpdateMusic(float value)
    {
        SaveAndLoadSystem.Instance.playerData.music = value;
    }

    private bool CheckLevelForPreviousFinishes(int stars)
    {
        if(SaveAndLoadSystem.Instance.playerData.levelsBeaten.ContainsKey(currentLevelID) && SaveAndLoadSystem.Instance.playerData.levelsBeaten[currentLevelID].starsEarned >= stars)
        {
            return false;
        }
        else return true;
    }

    public void GameCompleted()
    {
        SceneManager.LoadScene(8);
    }

    public void CheckForWin()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if(enemies.Length == 0)
        {
            Gamewin();
            lastWave = false;
        }
    }

    private void Gamewin()
    {
        int starsEarned = 0;

        // Determine how many stars are earned
        if(MoneyManager.instance.currentLives > 6)
        {
            starsEarned = 3;
        }
        else if(MoneyManager.instance.currentLives > 3)
        {
            starsEarned = 2;
        }
        else if(MoneyManager.instance.currentLives < 3)
        {
            starsEarned = 1;
        }

        int currentStars = 0;

        if(SaveAndLoadSystem.Instance.playerData.levelsBeaten.ContainsKey(currentLevelID))
        {
            currentStars = SaveAndLoadSystem.Instance.playerData.levelsBeaten[currentLevelID].starsEarned;
        }

        if(starsEarned > currentStars)
        {
            int newStars = starsEarned - currentStars;
            Debug.Log(newStars);
            SaveAndLoadSystem.Instance.playerData.stars += newStars;

            SaveAndLoadSystem.Instance.playerData.levelsBeaten[currentLevelID] = new LevelProgress("normal", starsEarned);

            UpdateStarText(starsEarned);
        }

        if(!SaveAndLoadSystem.Instance.playerData.levelsUnlocked.Contains(currentLevelID+1) && currentLevelID+1 != 3)
        {
            SaveAndLoadSystem.Instance.playerData.levelsUnlocked.Add(currentLevelID +1);
        }
        
        gameWonScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void GamePaused()
    {
        SoundFXManager.Instance.StopMusic();
        SaveAndLoadSystem.Instance.SavePlayerData();
        gameOptionsScreen.SetActive(false);
        gamePausedScreen.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
    public void Option()
    {
        SoundFXManager.Instance.StopMusic();
        gamePausedScreen.SetActive(false);
        gameOptionsScreen.SetActive(true);
    }

    public void GameResume()
    {
        SoundFXManager.Instance.PlayLevelMusic();
        gameOptionsScreen.SetActive(false);
        gamePausedScreen.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void RestartLevel()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }

    public void LeaveToMenu()
    {
        SoundFXManager.Instance.PlayMenuMusic();
        SaveAndLoadSystem.Instance.SavePlayerData();
        SceneManager.LoadScene(2);
    }

    //This method shows the place menu at the specified position
    public void showMenu(Vector2 newPosition)
    {
        placeMenu.SetActive(true);
    }

    // This method places a tower of the tower ID, when the button is pressed
    public void menuButton(int towerID)
    {
        for(int i = 0; i < placeMenuButtonsText.Count; i++)
        {
            Tower towerName = towerPlacer.towerPrefabs[i].GetComponent<Tower>();
            placeMenuButtonsText[i].text = towerName.towerData.towerName;
        }

        Tower towerScript = towerPlacer.towerPrefabs[towerID].GetComponent<Tower>();
        placeMenuButtonsText[towerID].text = towerScript.towerData.cost.ToString() + "$?";

        buttonPressedAmounts[towerID]++;
        for(int i = 0; i < buttonPressedAmounts.Count; i++)
        {
            if (i != towerID) 
            {
                buttonPressedAmounts[i] = 0;
            }
        }
        if(buttonPressedAmounts[towerID] > 1)
        {
            towerPlacer.TowerCanPlace(towerID);
            buttonPressedAmounts[towerID] = 0;
            placeMenuButtonsText[towerID].text = towerScript.towerData.towerName;
        }
    }
    
    // This method shows the upgrades for tower that is pressed on, which at the same time changes the text to be the correct upgrades for that tower.
    public void showUpgrades(Tower tower, Vector2 newPosition)
    {
        currentTower = tower;
        currentTowerPos = tower.transform.position;
        upGradeMenu.SetActive(true);

        /*
        Check tower index Id, then find its upgrades Ids
        With those at hand check if the player has unlocked them,
        use a foreach loop or a for loop to loop through which one the player has telling that button to turn on or not.
        */
        int towerGroup = tower.towerData.towerIDIndex;
        for(int i = 0; i < tower.upgrades.Count(); i++)
        {
            if(SaveAndLoadSystem.Instance.playerData.towersUnlocked[towerGroup].insideList.Contains(tower.upgrades[i].towerID))
            {
                UpgradeButtons[i].interactable = true;
            }
        }

        towerInfo.SetActive(true);
        showTowerRange(new Vector2(newPosition.x + tower.towerData.rangeIndicatorOffset.x, newPosition.y + tower.towerData.rangeIndicatorOffset.y));
        towerNameText.text = tower.towerData.towerName;

        towerToUpgradeText.text = currentTower.towerData.towerName;
        for(int i = 0; i < upgradeTexts.Count; i++)
        {
            if(currentTower.upgrades.Length >= upgradeTexts.Count)
            {
                upgradeTexts[i].text = currentTower.upgrades[i].upgradeName;
            }
            else
            {
                upgradeTexts[i].text = "TBA";
            }
        }
    }

    // Depending on the button pressed this method sends the number corresponding to the tower ID to be upgraded, which just places the new tower and deletes the old one.
    public void upGradeChoice(int choice)
    {
        int j = 0;
        foreach(TMP_Text upgradeText in upgradeTexts)
        {
            upgradeText.text = currentTower.upgrades[j].upgradeName;
            j++;
        }
        upgradeTexts[choice].text = currentTower.upgrades[choice].cost.ToString() + "$?";
        upgradeButtonPressedAmounts[choice]++;
        for(int i = 0; i < upgradeButtonPressedAmounts.Count; i++)
        {
            if (i != choice)
            {
                upgradeButtonPressedAmounts[i] = 0;
            }
        }
        if(currentTower != null && upgradeButtonPressedAmounts[choice] > 1 && MoneyManager.instance.MoneySpent(currentTower.upgrades[choice].cost))
        {
            currentTower.upGradeTower(choice, currentTowerPos);
            upgradeButtonPressedAmounts[choice] = 0;
            SoundFXManager.Instance.PlaySoundFXClip(TowerUpgradeFX);
            closeMenus();
        }
    }

    public void closeMenus()
    {
        placeMenu.SetActive(false);
        upGradeMenu.SetActive(false);
        towerInfo.SetActive(false);
        rangeImage.SetActive(false);
        towerPlacer.inMenu = false;
        for(int i = 0; i < placeMenuButtonsText.Count; i++)
        {
            Tower towerName = towerPlacer.towerPrefabs[i].GetComponent<Tower>();
            placeMenuButtonsText[i].text = towerName.towerData.towerName;
        }
        foreach(Button button in UpgradeButtons)
        {
            button.interactable = false;
        }
    }

    public void sellButton()
    {
        currentTower.sell();
        closeMenus();
    }

    private void showTowerRange(Vector2 position)
    {
        rangeImage.SetActive(true);
        rangeIndicator.position = position;
        rangeIndicator.sizeDelta = new Vector2(currentTower.towerData.range * 2, currentTower.towerData.range * 2);
    }

    public void showTowerPlacerIcon(int TowerID)
    {
        towerAboutToPlace = TowerID;
        placerIcon.SetActive(true);
        iconFollow = true;
    }

    public void Update()
    {
        if(lastWave)
        {
            CheckForWin();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                GameResume();
            }
            else
            {
                GamePaused();
            }
        }
        if(iconFollow == true)
        {
            // Get the tile type at the position of the mouse,
            // so we can decide wether a tower can be placed there.
            Vector3Int cellPosition = groundTiles.WorldToCell(towerPlacer.mouseWorldPos);
            Collider2D collider = Physics2D.OverlapPoint(towerPlacer.mouseWorldPos, 1);

            // The little icon that shows when you are trying to place a tower,
            // the math functions just round the values so they fits into the world grid.
            placerLocation.position = new Vector2((float)Math.Floor(towerPlacer.mouseWorldPos.x)+0.5f, (float)Math.Floor(towerPlacer.mouseWorldPos.y)+0.5f);

            if(towerAboutToPlace != 3 && buildableTiles.Contains(groundTiles.GetTile<Tile>(cellPosition)))
            {
                placerImage.color = Color.green;
            }
            else if(towerAboutToPlace == 3 && waterPlacableTiles.Contains(groundTiles.GetTile<Tile>(cellPosition)))
            {
                placerImage.color = Color.green;
            }
            else
            {
                placerImage.color = Color.red;
            }

            if(towerAboutToPlace != 3 && Input.GetMouseButton(0) && collider == null && (buildableTiles.Contains(groundTiles.GetTile<Tile>(cellPosition))))
            {
                towerPlacer.PlaceTower(towerAboutToPlace);
                iconFollow = false;
                placerIcon.SetActive(false);
                towerPlacer.inMenu = false;
            }
            else if(towerAboutToPlace == 3 && Input.GetMouseButton(0) && collider == null && (waterPlacableTiles.Contains(groundTiles.GetTile<Tile>(cellPosition))))
            {
                towerPlacer.PlaceTower(towerAboutToPlace);
                iconFollow = false;
                placerIcon.SetActive(false);
                towerPlacer.inMenu = false;
            }
            // Little secret on unlocking the ship tower.
            else if(towerAboutToPlace != 3 && Input.GetMouseButton(0) && collider == null && (waterPlacableTiles.Contains(groundTiles.GetTile<Tile>(cellPosition))) && !SaveAndLoadSystem.Instance.playerData.towersUnlocked[3].insideList.Contains(0))
            {
                SaveAndLoadSystem.Instance.playerData.towersUnlocked[3].insideList.Add(0);
                ResetPlaceButtons();
            }
        }
    }
}