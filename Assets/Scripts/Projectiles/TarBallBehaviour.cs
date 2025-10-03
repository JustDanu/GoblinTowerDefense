using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarBallBehaviour : MudBallBehaviour
{
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
            hitEnemyRefference.ApplyColorEffect(0.4f, 0.6f, 0.4f);
            CoroutineManager.Instance.StartCoroutine(speedReduce(effectDuration, hitEnemyRefference));
        }
        
    }
}
