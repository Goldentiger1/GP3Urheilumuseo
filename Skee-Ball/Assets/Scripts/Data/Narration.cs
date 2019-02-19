using System;
using UnityEngine;

[Serializable]
public class Narration : Sound
{
    public AudioClip[] AudioClips;

    public override void SetAudioSource(AudioSource audioSource)
    {
        base.SetAudioSource(audioSource);
        audioSource.outputAudioMixerGroup = AudioManager.Instance.GetChannelOutput("Narration");
    }

    public void PlayNarration()
    {
        if (audioSource.isPlaying == true)
            return;

        audioSource.clip = AudioClips[UnityEngine.Random.Range(0, AudioClips.Length)];
        NarrationPanel.Instance.ShowPanel(audioSource.clip.name);

        audioSource.Play();
        //Debug.LogWarning(Name + " PLAY ( Audio clip: " + audioSource.clip.name + " )");
    }

    public void StopNarration()
    {
        if (audioSource.isPlaying != true)
            return;

        audioSource.Stop();
        NarrationPanel.Instance.ClosePanel();
    }
}
