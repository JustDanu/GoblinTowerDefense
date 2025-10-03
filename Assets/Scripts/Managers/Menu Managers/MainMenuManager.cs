using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject buttons;
    [SerializeField]
    private GameObject options;

    [Header("Voume Sliders")]
    [SerializeField]

    private Slider volumeSlider;
    [SerializeField]
    private Slider musicSlider;

    public AudioSource musicObject;
    public void Start()
    {
        // Startup for the volume sliders
        InitialVolumeStartup();
        DontDestroyOnLoad(musicObject); 

        SoundFXManager.Instance.PlayMenuMusic();
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        SaveAndLoadSystem.Instance.SavePlayerData();
        Application.Quit();
    }
    public void Options(bool direction)
    {
        SaveAndLoadSystem.Instance.SavePlayerData();
        buttons.SetActive(!direction);
        options.SetActive(direction);
    }
    private void InitialVolumeStartup()
    {
        // Volume sliders are set to the saved values in the file
        volumeSlider.value = SaveAndLoadSystem.Instance.playerData.volume;
        musicSlider.value = SaveAndLoadSystem.Instance.playerData.music;

        // Add Listeners for all the sliders so they can change the values in the save system.
        volumeSlider.onValueChanged.AddListener(UpdateVolume);
        musicSlider.onValueChanged.AddListener(UpdateMusic);
    }

    public void UpdateVolume(float value)
    {
        SoundFXManager.Instance.UpdateVolumes();
        SaveAndLoadSystem.Instance.playerData.volume = value;
    }
    public void UpdateMusic(float value)
    {
        SoundFXManager.Instance.UpdateVolumes();
        SaveAndLoadSystem.Instance.playerData.music = value;
    }
    public void Tutorial()
    {
        SceneManager.LoadScene(7);
    }
}
