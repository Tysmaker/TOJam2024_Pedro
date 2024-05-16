using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

public class SoundManager : MonoBehaviour
{
    public List<SoundCategory> soundCategories = new List<SoundCategory>();

    //Gets reference to the scene so we can check the current build index and play the music accordingly.
    Scene scene;

    void Awake()
    {
        foreach (SoundCategory category in soundCategories)
        {
            foreach (Sound sound in category.backgroundMusic)
            {
                InitializeSound(sound);
            }
            foreach (Sound sound in category.soundEffects)
            {
                InitializeSound(sound);
            }
            foreach (Sound sound in category.ambientSounds)
            {
                InitializeSound(sound);
            }
            foreach (Sound sound in category.uiSounds)
            {
                InitializeSound(sound);
            }
        }
    }

    void InitializeSound(Sound sound)
    {
        sound.source = gameObject.AddComponent<AudioSource>();
        sound.source.clip = sound.clip;
        sound.source.loop = sound.loop;
        sound.source.volume = sound.volume;
        sound.source.pitch = sound.pitch;
    }

    void Start()
    {
        scene = SceneManager.GetActiveScene();
        PlayMusicForScene(scene.buildIndex);
    }

    void PlayMusicForScene(int buildIndex)
    {
        foreach (SoundCategory category in soundCategories)
        {
            foreach (Sound sound in category.backgroundMusic)
            {
                if (sound.sceneIndex == buildIndex)
                {
                    sound.source.Play();
                }
            }
        }
    }

    public void Play(string name)
    {
        foreach (SoundCategory category in soundCategories)
        {
            Sound sound = category.backgroundMusic.Find(s => s.name == name);
            if (sound != null)
            {
                sound.source.Play();
                return;
            }

            sound = category.soundEffects.Find(s => s.name == name);
            if (sound != null)
            {
                sound.source.Play();
                return;
            }

            sound = category.ambientSounds.Find(s => s.name == name);
            if (sound != null)
            {
                sound.source.Play();
                return;
            }

            sound = category.uiSounds.Find(s => s.name == name);
            if (sound != null)
            {
                sound.source.Play();
                return;
            }
        }
        Debug.LogWarning("Sound with name " + name + " not found!");
    }


    public void Stop(string name)
    {
        foreach (SoundCategory category in soundCategories)
        {
            Sound sound = category.backgroundMusic.Find(s => s.name == name);
            if (sound != null)
            {
                sound.source.Stop();
                return;
            }

            sound = category.soundEffects.Find(s => s.name == name);
            if (sound != null)
            {
                sound.source.Stop();
                return;
            }

            sound = category.ambientSounds.Find(s => s.name == name);
            if (sound != null)
            {
                sound.source.Stop();
                return;
            }

            sound = category.uiSounds.Find(s => s.name == name);
            if (sound != null)
            {
                sound.source.Stop();
                return;
            }
        }
        Debug.LogWarning("Sound with name " + name + " not found!");
    }


    public void Pause(string name)
    {
        foreach (SoundCategory category in soundCategories)
        {
            Sound sound = category.backgroundMusic.Find(s => s.name == name);
            if (sound != null)
            {
                sound.source.Pause();
                return;
            }

            sound = category.soundEffects.Find(s => s.name == name);
            if (sound != null)
            {
                sound.source.Pause();
                return;
            }

            sound = category.ambientSounds.Find(s => s.name == name);
            if (sound != null)
            {
                sound.source.Pause();
                return;
            }

            sound = category.uiSounds.Find(s => s.name == name);
            if (sound != null)
            {
                sound.source.Pause();
                return;
            }
        }
        Debug.LogWarning("Sound with name " + name + " not found!");
    }
}

[System.Serializable]
public class SoundCategory
{
    public List<Sound> backgroundMusic = new List<Sound>();
    public List<Sound> soundEffects = new List<Sound>();
    public List<Sound> ambientSounds = new List<Sound>();
    public List<Sound> uiSounds = new List<Sound>();
}