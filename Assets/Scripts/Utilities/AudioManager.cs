using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public List<Sound> sounds = new List<Sound>();

    void Awake()
    {
        Instance = this;

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            //sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    void Start()
    {
        // PlayAudio("MainTheme");
    }

    public void PlayAudio(string name)
    {
        Sound s = sounds.Find((x) => x.name == name);

        if (s == null)
        {
            Debug.Log("NotFound");
            return;
        }
        else
            Debug.Log("Found");

        s.source.Play();
    }
}
