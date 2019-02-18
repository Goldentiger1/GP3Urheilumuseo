using UnityEngine;
using UnityEngine.Audio;

public class Sound
{
    public bool IsPlaying
    {
        get
        {
            return audioSource.isPlaying;
        }
    }

    public string Name;
    protected AudioSource audioSource;
    protected AudioMixerGroup audioMixerGroup;

    [Range(0, 1)]
    public float volume = 0.5f;
    [Range(0.9f, 1.1f)]
    public float pitch = 1f;

    protected bool playOnAwake = false;

    public virtual void SetAudioSource(AudioSource audioSource)
    {
        this.audioSource = audioSource;

        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.playOnAwake = playOnAwake;
    }
}
