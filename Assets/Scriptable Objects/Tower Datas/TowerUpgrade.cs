using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tower Upgrade", menuName = "Tower Defense/TowerUpgrade", order = 1)]
public class TowerUpgrade : ScriptableObject
{
    public string upgradeName;
    public int towerIDIndex;
    public int towerID;
    public Vector2 placeOffSet;
    public int cost;
    public GameObject upgradePrefab;
}