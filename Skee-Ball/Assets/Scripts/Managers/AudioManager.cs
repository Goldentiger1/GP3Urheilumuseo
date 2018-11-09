using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singelton<AudioManager>
{
    private AudioMixer audioMixer;
    private AudioClip[] musicsTracks;
    private AudioClip[] sfxClips;

    private AudioSource backgroundMusicSource;

    private void Awake()
    {
        audioMixer = Resources.Load<AudioMixer>("Audio/Mixers/AudioMixer");
        musicsTracks = Resources.LoadAll<AudioClip>("Audio/Music/");
        sfxClips = Resources.LoadAll<AudioClip>("Audio/Sfx/");

        backgroundMusicSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        backgroundMusicSource.clip = GetMusicTrack("Background");
        backgroundMusicSource.loop = true;
        backgroundMusicSource.volume = 0.05f;

        backgroundMusicSource.Play();
    }

    private AudioClip GetMusicTrack(string trackName)
    {
        foreach (var musicTrack in musicsTracks)
        {
            if (musicTrack.name.Equals(trackName))
            {
                return musicTrack;
            }
        }

        Debug.LogError(trackName + " not found!");
        return null;
    }

    private AudioClip GetClip(string clipName)
    {
        foreach (var sfxClip in sfxClips)
        {
            if (sfxClip.name.Equals(clipName))
            {
                return sfxClip;
            }
        }

        Debug.LogError(clipName + " not found!");
        return null;
    }

    public void PlayClipAtPoint(string clipName, Vector3 point, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(GetClip(clipName), point, volume);
    }
}
