using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Sound
{

    public string name;
    public AudioClip clip;

    public bool loop;

    [Range(0.0f, 1.2f)]
    public float volume;

    [Range(0.0f, 1.1f)]
    public float pitch;

    public int sceneIndex;

    [HideInInspector]
    public AudioSource source;
}
