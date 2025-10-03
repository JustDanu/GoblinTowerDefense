using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Summonerenemy : Enemy
{
    public GameObject Placeholder;
    public GameObject SummonedEnemy;
    //I want to make a new enemy spawn every 5 seconds where the summoner enemy last was.
    protected override void Start()
    {
        base.Start();
        StartCoroutine(SummonEnemies());
    }
    protected IEnumerator SummonEnemies()
    {
        yield return new WaitForSeconds(3f);
        while(true)
        {
            Vector3 currentPosition = gameObject.transform.position;
            Placeholder = Instantiate(SummonedEnemy, currentPosition, Quaternion.identity);
            //Gains access to the Summoner enemies script and then location
            Summonedenemies summonerEnemyScript = Placeholder.GetComponent<Summonedenemies>();
            summonerEnemyScript.progress = progress;
            yield return new WaitForSeconds(5f);
        }
    }
    
}
