using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class TowerPlacer : MonoBehaviour
{
    //Audio clips!
    public AudioClip TowerPlacement;


    // Variables for accessing outside shit
    public UiManager UI;
    private int towerID;
    public List<GameObject> towerPrefabs;

    public Vector2 mouseWorldPos;
    public bool inMenu;
    public List<GameObject> ignoredGameObjects;

    // Update is called once per frame
    void Update()
    {
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        tileSelect();
    }
    // When you right click on a tile it gets the mouse position, then uses that position to get the tile at that position, then it uses that tile information
    // to check if the tile is a grass only tile, and if it is then it shows the menu from the UI manager.
    private void tileSelect()
    {
        if(Input.GetMouseButtonDown(1) && !inMenu)
        {
            inMenu = true;

            Collider2D collider = Physics2D.OverlapPoint(mouseWorldPos, 1);

            UiManager.instance.showMenu(new Vector2(Mathf.FloorToInt(mouseWorldPos.x)+0.5f, Mathf.FloorToInt(mouseWorldPos.y)+0.5f));
        }
        else if (Input.GetMouseButtonDown(0) && !inMenu)
        {
            inMenu = true;
            
            Collider2D collider = Physics2D.OverlapPoint(mouseWorldPos, 1);
            if(collider != null)
            {
                Tower clickedTower = collider.gameObject.GetComponent<Tower>();
                Vector2 clickedTowerPosition = collider.gameObject.transform.position;
                UiManager.instance.showUpgrades(clickedTower, clickedTowerPosition);
            }
            else
            {
                inMenu = false;
            }
        }
        else if(Input.GetMouseButtonDown(0) && inMenu)
        {
            if(EventSystem.current.currentSelectedGameObject == null)
            {
                inMenu = false;
                UiManager.instance.closeMenus();
                
            }
        }
    }

    public void PlaceTower(int towerID)
    {
        Tower beingPlaced = towerPrefabs[towerID].GetComponent<Tower>();

        Instantiate(towerPrefabs[towerID], new Vector3((float)(Math.Floor(mouseWorldPos.x)+0.5f+beingPlaced.towerData.placeOffSet.x), ((float)Math.Floor(mouseWorldPos.y))+0.5f+beingPlaced.towerData.placeOffSet.y), Quaternion.identity);
        SoundFXManager.Instance.PlaySoundFXClip(TowerPlacement);
    }
    // This method gets the prefab information from an index from the list of prefabs and places it at the current mouse position.
    public void TowerCanPlace(int towerID)
    {
        if(towerPrefabs[towerID] != null)
        {
            Tower beingPlaced = towerPrefabs[towerID].GetComponent<Tower>();
            if(MoneyManager.instance.MoneySpent(beingPlaced.towerData.cost))
            {
                UiManager.instance.showTowerPlacerIcon(towerID);
                UiManager.instance.placeMenu.SetActive(false);
            }
        }
        else
        {
            Debug.Log("Tower ID not found");
        }
    }
}
