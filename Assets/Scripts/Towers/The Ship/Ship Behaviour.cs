using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityRandom = UnityEngine.Random;

public class ShipBehaviour : Tower
{
    [SerializeField]
    protected List<Tile> placableTiles;
    protected Tilemap groundTiles;
    protected override void Start()
    {
        animator = GetComponentInChildren<Animator>();
        towerTransform = GetComponent<Transform>();

        groundTiles = FindAnyObjectByType<Tilemap>();

        // Finds and sets up the tower placer script to the variable
        GameObject towerPlacerObject = GameObject.Find("TowerPlacer");
        towerPlacer = towerPlacerObject.GetComponent<TowerPlacer>();

        //To be changed
        upgradePathIndex = 0;

        StartCoroutine(SummonTowers());
    }

    protected virtual IEnumerator SummonTowers()
    {
        while(true)
        {
            yield return new WaitForSeconds(towerData.fireRate);

            Vector3 randomPosition = new Vector3(transform.position.x + UnityRandom.Range(-towerData.range, towerData.range), transform.position.y + UnityRandom.Range(-towerData.range, towerData.range));
            Debug.Log(randomPosition);
            Vector3Int cellPosition = groundTiles.WorldToCell(randomPosition);
            Collider2D collider = Physics2D.OverlapPoint(randomPosition, 1);

            if(placableTiles.Contains(groundTiles.GetTile<Tile>(cellPosition)) && collider == null)
            {
                Instantiate(towerData.projectilePrefab, new Vector3((float)(Math.Floor(randomPosition.x)+0.5f), ((float)Math.Floor(randomPosition.y))+0.5f), Quaternion.identity);
            }
        }
    }

    
}
