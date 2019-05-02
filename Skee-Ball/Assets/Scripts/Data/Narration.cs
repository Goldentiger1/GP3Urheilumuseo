using System;
using UnityEngine;

[Serializable]
public class Narration : Sound
{
    public AudioClip[] AudioClips_FI;
    public AudioClip[] AudioClips_UK;

    public override void SetAudioSource(AudioSource audioSource)
    {
        base.SetAudioSource(audioSource);
        audioSource.outputAudioMixerGroup = AudioManager.Instance.GetChannelOutput("Narration");
    }

    private AudioClip GetRandomNarration(AudioClip[] languageAudioClips)
    {
        if(languageAudioClips.Length > 0)
        {
            return languageAudioClips[UnityEngine.Random.Range(0, languageAudioClips.Length)];
        }

        return AudioPlayer.Instance.GetSoundEffect(2, "MissingAudioClip");          
    }

    public void PlayNarration()
    {
        if (audioSource.isPlaying == true)
            return;

        switch (LocalizationManager.Instance.CURRENT_LANGUAGE)
        {
            case LANGUAGE.FI:

            audioSource.clip = GetRandomNarration(AudioClips_FI);

            break;

            case LANGUAGE.UK:

            audioSource.clip = GetRandomNarration(AudioClips_UK);

            break;

            default:

            break;
        }
        
        NarrationPanel.Instance.ShowPanel(audioSource.clip.name);
        audioSource.Play();
        //Debug.LogWarning(Name + " PLAY ( Audio clip: " + audioSource.clip.name + " )");
    }

    public void StopNarration()
    {
        if (audioSource.isPlaying == false)
            return;

        audioSource.Stop();
        NarrationPanel.Instance.ClosePanel();
    }
}
