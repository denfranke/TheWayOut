using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    [HideInInspector] public AudioSource audioSource;

    [Range(0, 1)] public float volume;
    [Range(.1f, 3f)] public float pitch;

    public AudioClip clip;

    public string name;
    public bool loop;
}
