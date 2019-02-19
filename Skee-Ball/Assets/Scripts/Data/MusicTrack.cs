using System;
using UnityEngine;

[Serializable]
public class MusicTrack : Sound
{
    public AudioClip audioClip;
    public bool loop = false;

    public override void SetAudioSource(AudioSource audioSource)
    {
        base.SetAudioSource(audioSource);
        audioSource.clip = audioClip;
        audioSource.loop = loop;
        audioSource.outputAudioMixerGroup = AudioManager.Instance.GetChannelOutput("Music");
    }

    public void PlayTrack()
    {
        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    public void StopTrack()
    {
        if (audioSource.isPlaying)
            audioSource.Stop();
    }
}
