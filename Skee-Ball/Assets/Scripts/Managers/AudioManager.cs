using System;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class Sound
{
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

[Serializable]
public class Sfx : Sound
{
    public AudioClip audioClip;
}

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
        Debug.LogError(audioSource.clip.name);
        NarrationPanel.Instance.ShowPanel(audioSource.clip.name);

        audioSource.Play();
        Debug.LogWarning(Name + " PLAY ( Audio clip: " + audioSource.clip.name + " )");
    }

    public void StopNarration()
    {
        if (audioSource.isPlaying != true)
            return;

        audioSource.Stop();
        Debug.LogWarning(Name + " STOP");
    }
}

public class AudioManager : Singelton<AudioManager>
{
    public AudioMixer AudioMixer;   
    public AudioMixerUpdateMode AudioMixerUpdateMode
    {
        get;
        private set;
    }
    public AudioMixerGroup[] AudioMixerGroups
    {
        get;
        private set;
    }

    public bool IsAudioFading
    {
        get;
        private set;
    }

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        AudioMixerUpdateMode = AudioMixerUpdateMode.UnscaledTime;
        AudioMixerGroups = AudioMixer.FindMatchingGroups(string.Empty);
    }

    private float DecibelToLinearValue(float decibelValue)
    {
        float linearValue = Mathf.Pow(10.0f, decibelValue / 20.0f);

        return linearValue;
    }

    private float LinearToDecibelValue(float linearValue)
    {
        return linearValue != 0 ? 20.0f * Mathf.Log10(linearValue) : -80;
    }

    public float GetChannelValue(string channelName)
    {
        float value;
        AudioMixer.GetFloat(channelName, out value);
        return value;
    }

    public AudioMixerGroup GetChannelOutput(string outputName)
    {
        foreach (AudioMixerGroup output in AudioMixerGroups)
        {
            if (output.name == outputName)
            {
                return output;
            }
        }

        return null;
    }

    public void SetLowPassValue(float newValueInHertz)
    {
        AudioMixer.SetFloat("LowPassValue", newValueInHertz);
    }

    public void SetAudioMixerChannelValue(string channelParameterName, float value)
    {
        float valueInDecibel = LinearToDecibelValue(value);
        AudioMixer.SetFloat(channelParameterName, valueInDecibel);
    }

    public void FadeChannelVolume(string channelParameterName, float targetVolume, float fadeTime)
    {
        StartCoroutine(IFadeVolume(channelParameterName, targetVolume, fadeTime));
    }

    private IEnumerator IFadeVolume(string channelParameterName, float targetVolume, float fadeDuration)
    {
        yield return new WaitUntil(() => IsAudioFading == false);
        float startChannelVolume = GetChannelValue(channelParameterName);

        float startLerpTime = Time.unscaledTime;
        float timeSinceStarted = Time.unscaledTime - startLerpTime;
        float percentToComplete = timeSinceStarted / fadeDuration;

        targetVolume = LinearToDecibelValue(targetVolume);

        if (targetVolume != startChannelVolume)
        {
            IsAudioFading = true;

            while (true)
            {
                timeSinceStarted = Time.unscaledTime - startLerpTime;
                percentToComplete = timeSinceStarted / fadeDuration;

                float currentVolume = Mathf.Lerp(startChannelVolume, targetVolume, percentToComplete);
                AudioMixer.SetFloat(channelParameterName, currentVolume);

                if (percentToComplete > 1f)
                {
                    IsAudioFading = false;
                    break;
                }

                yield return new WaitForEndOfFrame();
            }
        }
    }
}
