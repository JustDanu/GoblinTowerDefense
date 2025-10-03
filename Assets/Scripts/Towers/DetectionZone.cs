using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public Tower parentTower;
    private List<Transform> targetsInRange = new List<Transform>();

    private void Start()
    {
        if (parentTower == null)
        {
            parentTower = GetComponentInParent<Tower>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            targetsInRange.Add(other.transform);
            parentTower.AddTarget(other.transform);

            if(targetsInRange.Count >= 1 && !parentTower.isShooting)
            {
                parentTower.StartShooting();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            targetsInRange.Remove(other.transform);
            parentTower.RemoveTarget(other.transform);

            if (targetsInRange.Count == 0)
            {
                parentTower.StopShooting();
            }
        }
    }
}
