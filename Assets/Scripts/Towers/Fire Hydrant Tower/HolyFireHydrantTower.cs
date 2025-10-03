using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HolyFireHydrantTower : Tower
{
    private List<Projectile> lines = new List<Projectile>();
    private List<LineRenderer> lineRenders = new List<LineRenderer>();

    protected override void Start()
    {
        base.Start();
        lineRenders = GetComponentsInChildren<LineRenderer>().ToList();
        foreach(LineRenderer lineRenderer in lineRenders)
        {
            lines.Add(lineRenderer.GetComponent<Projectile>());
        }
    }

    protected override void shoot()
    {
        Vector2 firePointPosition = CalculateFirePoint();
        for(int i = 0; i < lines.Count && i < targetsInRange.Count; i++)
        {
            lineRenders[i].enabled = true;
        }
        
        for(int i = 0; i < lines.Count && i < targetsInRange.Count; i++)
        {
            lines[i].findTarget(targetsInRange[i]);
        }
    }
    protected override IEnumerator shootDelay()
    {
        while(targetsInRange.Count > 0)
        {
            yield return new WaitForSeconds(towerData.fireRate);
            if(targetsInRange.Count > 0)
            {
                target = targetsInRange[0];
                shoot();
            }
        }
    }
}
