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
        
    }

    private void StopMusicTrack()
    {
        
    }

    private void StopNarration()
    {
        
    }

    public void PlaySfx(string sfxName)
    {
        
    }

    public void ChangeMusicTrack(int index)
    {

        StopMusicTrack();
        PlayMusicTrack(GetCorrectSceneMusicTrack(index));
        Debug.Log("ChangeMusicTrack: " + GetCorrectSceneMusicTrack(index));
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

            default:

                Debug.LogError("Index music not assigned... Index: " + index + " not found!");

                return "Null";
        }
    }
}
