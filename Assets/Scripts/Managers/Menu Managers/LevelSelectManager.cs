using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    public void WorldSelect(int world)
    {
        SceneManager.LoadScene(world);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
