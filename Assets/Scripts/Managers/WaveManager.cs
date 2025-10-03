using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

public class WaveManager : MonoBehaviour
{
    public CountdownTimer Countdown;
    public GameObject BasicEnemy;
    public GameObject TankyEnemy;
    public GameObject FastEnemy;
    public GameObject SummonerEnemy;
    public UiManager UI;

    private Vector3 spawnPosition;

    public TMP_Text WaveCountdown;

    public TMP_Text CD_text;

    public TMP_Text CurWave;

    public GameObject StartLocation;

    private int currentWave = 0;

    public int totalWaveCount;

    public bool secondLastWaveCheck = false;
    public bool LastWaveCheck = false;
    private int currentWavePlusOne = 1;

    [SerializeField]
    public List<InnerList> Waves;

    [System.Serializable]
    public class InnerList
    {
          public List<Enemy> Enemies;
    }
    public void Start()
    {
        totalWaveCount = Waves.Count;
        spawnPosition = StartLocation.transform.position;
        StartCoroutine(EnemyWaves());
    }
    //I need to call each wave in and then check to see if all enemies have spawned, in which a countdown will run and when the countdown finishes the next wave is called.
    public IEnumerator EnemyWaves()
    {
        Debug.Log(totalWaveCount);
        while(currentWave <= Waves.Count)
        {
            if(secondLastWaveCheck)
            {
                LastWaveCheck = true;
            }
            //if it is the second last wave
            if(currentWave == totalWaveCount - 2)
            {
                CD_text.text = "Final Wave: ";
                secondLastWaveCheck = true;
                yield return StartCoroutine(SpawnWave(Waves[currentWave]));

            }
            if(LastWaveCheck)
            {
                UI.lastWave = true;
                break;
            }
            
            yield return StartCoroutine(SpawnWave(Waves[currentWave]));

        }
        

    }

    public IEnumerator SpawnWave(InnerList wave)
    {

        CurWave.text = "Wave: " + currentWavePlusOne.ToString();

        foreach (var enemyPrefab in wave.Enemies)
        {
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(2f);
        }
       //if it is not the final wave
        if(secondLastWaveCheck == false)
        {
            StartCoroutine(Countdown.Timer(10));
            yield return new WaitForSeconds(9f); 
        }
        else//If the final wave is next
        {
            //if it is not the end of the final wave then run the countdown timer
            if(currentWave != totalWaveCount - 1)
            {
                StartCoroutine(Countdown.Timer(10));
                yield return new WaitForSeconds(9f); 
            }

        }
        
        currentWave++;
        currentWavePlusOne = currentWave + 1; 
        
    }
}
