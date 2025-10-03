using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterLaserBehaviour : Projectile
{
    protected LineRenderer lineRenderer;
    protected Transform towerPosition;
    protected bool isTargeting;
    protected int currentDamage;
    protected Tower parentTower;
    protected Transform previousTarget;
    protected int damageIncreaseRate;

    protected override void Start()
    {
        isTargeting = false;
        parentTower = GetComponentInParent<Tower>();
        towerPosition = GetComponentInParent<Transform>();
        lineRenderer = GetComponent<LineRenderer>();

        damageIncreaseRate = 1;
    }
    protected override void Update()
    {
        if(parentTower.targetsInRange.Count == 0)
        {
            target = null;
        }
        if(target != null)
        {
            lineRenderer.SetPosition(0, towerPosition.position);

            Vector3 endPosition = new Vector3(target.position.x, target.position.y, 0);
            lineRenderer.SetPosition(1, endPosition);
            if(!isTargeting)
            {
                previousTarget = target;
                StartCoroutine(dealDamage());
                isTargeting = true;
            }
        }
        else
        {
            lineRenderer.enabled = false;
            isTargeting = false;
        }

    }

    protected IEnumerator dealDamage()
    {
        currentDamage = projectileData.damage;

        while(target != null && parentTower.targetsInRange.Contains(target))
        {
            if(target != null && previousTarget != null && target.name != previousTarget.name)
            {
                break;
            } 
            targetHit();
            if(currentDamage < 3) currentDamage += damageIncreaseRate;

            yield return new WaitForSeconds(1.5f);
        }
        isTargeting = false;
    }

    protected override void targetHit()
    {
        Enemy hitEnemyRefference = target.gameObject.GetComponent<Enemy>();
        hitEnemyRefference.currentHp -= currentDamage;
    }
}
