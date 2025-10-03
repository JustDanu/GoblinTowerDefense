using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleWaterLaserBehaviour : WaterLaserBehaviour
{
    protected override void Start()
    {
        isTargeting = false;
        parentTower = GetComponentInParent<Tower>();
        towerPosition = GetComponentInParent<Transform>();
        lineRenderer = GetComponent<LineRenderer>();

        damageIncreaseRate = 0;
    }
}
