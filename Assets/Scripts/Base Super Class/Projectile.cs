using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public ProjectileData projectileData;
    protected Transform target;
    protected Transform projectile;

    // Animation Values
    protected float timeElapsed;
    protected float startScale = 1f;
    protected float endScale = 0.5f;
    protected float currentSpeed;

    public virtual void findTarget(Transform _target)
    {
        target = _target;
    }

    protected virtual void Start()
    {
        currentSpeed = projectileData.speed;
        projectile = GetComponent<Transform>();
    }

    protected virtual void Update()
    {
        
        if(target != null)
        {
            Vector2 direction = (Vector2)target.position - (Vector2)projectile.position;
            float distanceThisFrame = currentSpeed * Time.deltaTime;
            currentSpeed *= 1.005f;


            if(direction.magnitude <= 0.5f)
            {
                transform.localScale = Vector3.one * 0.5f;
                targetHit();
                return;
            }

            transform.Translate(direction.normalized * distanceThisFrame, Space.World);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            
        }
        else
        {
            Destroy(gameObject);
        }

    }

    protected virtual void targetHit()
    {
        Enemy hitEnemyRefference = target.gameObject.GetComponent<Enemy>();
        hitEnemyRefference.currentHp -= projectileData.damage;
        Destroy(gameObject);
    }
}
