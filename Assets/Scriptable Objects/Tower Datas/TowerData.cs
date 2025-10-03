using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tower", menuName = "Tower Defense/Tower")]
public class TowerData : ScriptableObject
{
    public string towerName;
    public int towerIDIndex;
    public int towerID;
    public Vector2 placeOffSet;
    public Vector2 rangeIndicatorOffset;
    public float range;
    public float fireRate;
    public int cost;
    public Vector2 firePoint;
    public GameObject projectilePrefab;
}
