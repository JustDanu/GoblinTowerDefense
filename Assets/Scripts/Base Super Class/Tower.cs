using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Tower : MonoBehaviour
{
    //Audio clip!
    public AudioClip projectileThrowSound;

    // Tower upgrades variables
    public TowerData towerData;
    public TowerUpgrade[] upgrades;
    public int upgradePathIndex;

    // Tower placer script variable
    public TowerPlacer towerPlacer;
    
    public bool isShooting;

    protected Transform towerTransform;
    protected CircleCollider2D detectionZone;
    public List<Transform> targetsInRange = new List<Transform>();
    protected Transform target;

    protected Animator animator;

    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();

        detectionZone = GetComponentInChildren<CircleCollider2D>();
        towerTransform = GetComponent<Transform>();

        detectionZone.isTrigger = true;
        detectionZone.radius = towerData.range;

        // Finds and sets up the tower placer script to the variable
        GameObject towerPlacerObject = GameObject.Find("TowerPlacer");
        towerPlacer = towerPlacerObject.GetComponent<TowerPlacer>();

        //To be changed
        upgradePathIndex = 0;

    }

    protected virtual void shoot()
    {
        Vector2 firePointPosition = CalculateFirePoint();
        GameObject pewPew = Instantiate(towerData.projectilePrefab, firePointPosition, Quaternion.identity);
        Projectile projectile = pewPew.GetComponent<Projectile>();

        projectile.findTarget(target);
    }

    // Upgrades this tower, to either of the options, which is determined at the button press
    public virtual void upGradeTower(int path, Vector2 towerPosition)
    {
        Destroy(gameObject);
        Instantiate(upgrades[path].upgradePrefab, new Vector2(towerPosition.x + upgrades[path].placeOffSet.x, towerPosition.y + upgrades[path].placeOffSet.y), Quaternion.identity);
        towerPlacer.inMenu = false;
    }

    public virtual void sell()
    {
        MoneyManager.instance.MoneyEarned(towerData.cost/2);
        Destroy(gameObject);
    }

    public void AddTarget(Transform target)
    {
        targetsInRange.Add(target);
    }

    public void RemoveTarget(Transform target)
    {
        targetsInRange.Remove(target);
    }

    public void StartShooting()
    {
        StartCoroutine(shootDelay());
    }

    public void StopShooting()
    {
        StopCoroutine(shootDelay());
    }

    protected Vector2 CalculateFirePoint()
    {
        return new Vector2(towerTransform.position.x - towerData.firePoint.x, towerTransform.position.y - towerData.firePoint.y);
    }

    protected virtual IEnumerator shootDelay()
    {
        isShooting = true;
        while(targetsInRange.Count > 0)
        {
            target = targetsInRange[0];
            Enemy EnemyRefference = target.GetComponentInParent<Enemy>();
            if(EnemyRefference.isDead == true)
            {
                RemoveTarget(target);
                continue;
            }
            if(animator != null)
            {
                animator.SetTrigger("hasShot");
            }
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            float animationDuration = stateInfo.length / stateInfo.speed;

            yield return new WaitForSeconds(animationDuration);
        }
        isShooting = false;
    }

    public void ThrowSound()
    {
        SoundFXManager.Instance.PlaySoundFXClip(projectileThrowSound);
    }

}
