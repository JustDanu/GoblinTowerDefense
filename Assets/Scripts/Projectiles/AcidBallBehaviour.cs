using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidBallBehaviour : Projectile
{
    private float effectDuration = 3f;

    protected override void targetHit()
    {
        Enemy hitEnemyRefference = target.gameObject.GetComponent<Enemy>();
        Destroy(gameObject);

        if(!hitEnemyRefference.isPoisoned)
        {
            // Sets the color to be greener
            hitEnemyRefference.ApplyColorEffect(0.6f, 1f, 0.6f);
            hitEnemyRefference.isPoisoned = true;
            CoroutineManager.Instance.StartCoroutine(poisonedEffect(effectDuration, hitEnemyRefference));
        }
        
    }

    protected IEnumerator poisonedEffect(float duration, Enemy enemy)
    {
        float elapsed = 0f;
        while(elapsed <= duration)
        {
            enemy.currentHp -= projectileData.damage;
            yield return new WaitForSeconds(1f);
            elapsed += 1f;
        }
        enemy.isPoisoned = false;
        // Color back to normal
        enemy.ApplyColorEffect(1f, 1f, 1f);
    }
}
