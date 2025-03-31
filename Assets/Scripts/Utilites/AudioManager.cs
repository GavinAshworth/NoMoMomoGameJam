using System;
using Unity.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Sounds[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("Earth Level"); // Make sure to change to menu
    }


    public void PlayMusic(string name) {
        Sounds s = Array.Find(musicSounds, x=> x.name == name);

        if(s == null) Debug.Log("Sound Not Found");
        else {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name) {
        Sounds s = Array.Find(sfxSounds, x=> x.name == name);

        if(s == null) Debug.Log("Sound Not Found");
        else {
            sfxSource.PlayOneShot(s.clip);
        }
    }
}
