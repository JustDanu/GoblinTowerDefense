using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Tower Defense/Enemy")]
public class EnemyData : ScriptableObject
{    
    public string enemyName;
    public float speed;
    public int health;
    public int livesTaken;
    public int moneyGained;
}
