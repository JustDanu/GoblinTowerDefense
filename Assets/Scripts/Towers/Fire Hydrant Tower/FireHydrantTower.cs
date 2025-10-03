using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireHydrantTower : Tower
{

    private LineRenderer line;

    protected override void Start()
    {
        base.Start();
        line = GetComponentInChildren<LineRenderer>();
    }
    protected override void shoot()
    {
        Vector2 firePointPosition = CalculateFirePoint();
        line.enabled = true;
        Projectile lineBehaviour = line.GetComponent<Projectile>();

        lineBehaviour.findTarget(target);
    }

    protected override IEnumerator shootDelay()
    {
        while(targetsInRange.Count > 0)
        {
            yield return new WaitForSeconds(towerData.fireRate);
            if(targetsInRange.Count > 0)
            {
                target = targetsInRange[0];
                if(animator != null)
                {
                    animator.SetTrigger("hasShot");
                    yield return new WaitForSeconds(1f);
                }
                shoot();
            }
            else line.enabled = false;
        }
    }
}
