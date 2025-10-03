using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Mathematics;

public class HeavyRockBehaviour : Projectile
{
    public GameObject explosionEffect;
    private float effectRadius = 1.5f;

    public AudioClip heavyRockBoom;
    protected override void Start()
    {
        base.Start();
        Destroy(gameObject, 3f);
    }
    protected override void targetHit()
    {
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(transform.position, effectRadius);
        GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(explosion, 1f);
        SoundFXManager.Instance.PlaySoundFXClip(heavyRockBoom);

        foreach(Collider2D enemyCollider in enemyColliders)
        {
            //This if statement checks if the enemy collider is null, just in case.
            if(enemyCollider.gameObject != null)
            {
                Enemy hitEnemyRefference = enemyCollider.gameObject.GetComponent<Enemy>();
                if(hitEnemyRefference != null)
                {
                    Debug.Log(hitEnemyRefference.enemyData.enemyName + " took damage "+projectileData.damage/3);
                    hitEnemyRefference.currentHp -= projectileData.damage/3;
                }
            }
            
        }
        Destroy(gameObject);
    }
}

