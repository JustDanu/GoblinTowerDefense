using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile", menuName = "Tower Defense/Projectile")]
public class ProjectileData : ScriptableObject
{    
    public string projectileName;
    public float speed;
    public int damage;
}
