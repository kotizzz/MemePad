using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer Instance = null;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource _musicAudioSource;

    [Header("Sound")]
    [SerializeField] private Sound[] _sounds;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void PlaySound(string name)
    {
        if(_musicAudioSource.isPlaying == false)
        {
            Sound s = Array.Find(_sounds, sound => sound.name == name);

            if (s == null)
            {
                Debug.LogError(name + " not found.");
                return;
            }

            _musicAudioSource.PlayOneShot(s.audioClip);
        }
    }
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip audioClip;
}
