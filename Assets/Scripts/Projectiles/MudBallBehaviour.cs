using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudBallBehaviour : Projectile
{
    protected float originalEnemySpeed;
    protected float reducedEnemySpeed;

    protected float effectDuration = 3f;

    protected override void targetHit()
    {
        Enemy hitEnemyRefference = target.gameObject.GetComponent<Enemy>();
        Destroy(gameObject);

        if(!hitEnemyRefference.isSlowed)
        {
            originalEnemySpeed = hitEnemyRefference.currentSpeed;
            reducedEnemySpeed = (originalEnemySpeed / projectileData.damage);
            hitEnemyRefference.isSlowed = true;
            // Sets the color to be darker
            hitEnemyRefference.ApplyColorEffect(0.6f, 0.6f, 0.6f);
            CoroutineManager.Instance.StartCoroutine(speedReduce(effectDuration, hitEnemyRefference));
        }
        
    }

    protected IEnumerator speedReduce(float duration, Enemy enemy)
    {
        enemy.currentSpeed = reducedEnemySpeed;

        yield return new WaitForSeconds(duration);

        enemy.isSlowed = false;
        enemy.currentSpeed = originalEnemySpeed;
        enemy.ApplyColorEffect(1f, 1f, 1f);
    }
}
