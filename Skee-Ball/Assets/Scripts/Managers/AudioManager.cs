using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioManager : Singelton<AudioManager>
{
    #region VARIABLES

    public AudioMixer AudioMixer;

    #endregion VARIABLES

    #region PROPERTIES

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

    #endregion PROPERTIES

    #region UNITY_FUNCTIONS

    private void Awake()
    {
        Initialize();
    }

    #endregion UNITY_FUNCTIONS

    #region CUSTOM_FUNCTIONS

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

    #endregion CUSTOM_FUNCTIONS
}
