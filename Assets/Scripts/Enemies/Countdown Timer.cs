using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    public GameObject CD;
    [SerializeField] TextMeshProUGUI Countdown;

    [SerializeField] float remainingTime;

    public IEnumerator Timer(float initialTime)
    {
        remainingTime = initialTime;
        CD.SetActive(true);
        while(remainingTime > 1)
        {
            remainingTime -= Time.deltaTime;
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            Countdown.text = string.Format("{0:00}", seconds);
            yield return Countdown.text;
        }
        CD.SetActive(false);
    }
}

