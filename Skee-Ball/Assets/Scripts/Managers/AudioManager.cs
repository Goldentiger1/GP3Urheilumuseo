using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singelton<AudioManager>
{
    private AudioMixer audioMixer;

    private void Awake()
    {
        audioMixer = Resources.Load<AudioMixer>("Mixers/AudioMixer");
    }

    private void PlayMusicTrack(string musicTrackName)
    {
        Fabric.EventManager.Instance.PostEvent(musicTrackName);
    }

    private void StopMusicTrack()
    {
        Fabric.EventManager.Instance.PostEvent("Stop_Music");
    }

    private void StopNarration()
    {
        Fabric.EventManager.Instance.PostEvent("Stop_Narration");
    }

    public void PlaySfx(string sfxName)
    {
        Fabric.EventManager.Instance.PostEvent(sfxName);
    }

    public void ChangeMusicTrack(int index)
    {
        StopMusicTrack();
        PlayMusicTrack(GetCorrectSceneMusicTrack(index));
    }

    private string GetCorrectSceneMusicTrack(int index)
    {
        switch (index)
        {
            case 0:

                return "Menu";

            case 1:

                return "Ambient";

            case 2:

                return "Street";

            case 3:

                return "Menu";

            default:

                Debug.LogError("Index music not assigned... Index: " + index + " not found!");

                return "Null";
        }
    }
}
