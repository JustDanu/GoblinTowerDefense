using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    public GameObject stepOne;
    public GameObject stepTwo;
    public void NextStep()
    {
        stepOne.SetActive(false);
        stepTwo.SetActive(true);
    }

    public void Back()
    {
        SceneManager.LoadScene(0);
    }
}
