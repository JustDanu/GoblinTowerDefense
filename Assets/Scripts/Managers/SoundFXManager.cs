using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance;
    public AudioSource music;
    public AudioClip menuMusicClip;
    public AudioClip levelMusicClip;

    [SerializeField] private AudioSource soundFXObject;
    private void Awake()
    {
        if(Instance == null)
        {
            //sets the instance to equal this script
            Instance = this;

            // So its the same gameobject across scenes
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        PlayMenuMusic();
    }

//I need a method that allows me to change the pitch of a given audioclip to something random 

    public void PlaySoundFXClip(AudioClip audioClip)
    {
        //spawn in gameobject
        AudioSource audioSource = Instantiate(soundFXObject);

        //assign the audioClip
        audioSource.clip = audioClip;

        //assign volume
        audioSource.volume = SaveAndLoadSystem.Instance.playerData.volume;

        //randomize pitch
        audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f); 

        //play sound
        audioSource.Play();

        //get lenth of sound FX clip
        float clipLength = audioSource.clip.length;

        //Destroy this gameObject after the clip is done
        Destroy(audioSource.gameObject, clipLength + 3f);
    }

    public void PlayMenuMusic()
    {
        if(music != menuMusicClip)
        {
            music.clip = menuMusicClip;
            music.volume = SaveAndLoadSystem.Instance.playerData.music;
            music.Play();
        }
    }
    public void PlayLevelMusic()
    {
        if(music != levelMusicClip)
        {
            music.clip = levelMusicClip;
            music.volume = SaveAndLoadSystem.Instance.playerData.music;
            music.Play();
        }
        
    }
    public void UpdateVolumes()
    {
        music.volume = SaveAndLoadSystem.Instance.playerData.music;
    }
    public void StopMusic()
    {
        music.Stop();
    }
}
