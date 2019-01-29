using UnityEngine;
using UnityEngine.Audio;

public class Sound
{
    private AudioSource audioSource;

    public Sound(AudioSource audioSource)
    {
        this.audioSource = audioSource;

        audioSource.playOnAwake = false;
    }

    public void PlaySound()
    {
        if(audioSource.isPlaying == false)
        {
            audioSource.Play();
        }
    }
}

public class AudioManager : Singelton<AudioManager>
{
    private AudioMixer audioMixer;

    private AudioClip[] musicTracks;
    private AudioClip[] soundEffects;

    private AudioMixerGroup[] audioMixerGroupOutputs;

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        CreateSounds();
    }

    private void Initialize()
    {
        audioMixer = Resources.Load<AudioMixer>("Audio/Mixers/AudioMixer");

        musicTracks = Resources.LoadAll<AudioClip>("Audio/MusicTracks/");
        soundEffects = Resources.LoadAll<AudioClip>("Audio/SoundEffects/");

        audioMixerGroupOutputs = audioMixer.FindMatchingGroups("Master");
    }

    private void CreateSounds()
    {
        var musicTrackContainer = new GameObject("MusicTracks").transform;
        var soundEffectContainer = new GameObject("SoundEffects").transform;
        var narrationContainer = new GameObject("Narrations").transform;

        musicTrackContainer.parent = soundEffectContainer.parent = transform;

        foreach (var musicTrack in musicTracks)
        {
            CreateAudioSource(musicTrackContainer, musicTrack);         
        }
    }

    private void CreateAudioSource(Transform parent, AudioClip audioClip)
    {
        var newAudioClipGameObject = new GameObject(audioClip.name + "_MusicTrack");

        var audioSource = newAudioClipGameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.outputAudioMixerGroup = audioMixerGroupOutputs[1];

        newAudioClipGameObject.transform.SetParent(parent);
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

    public void PlaySfx(string sfxName, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(GetSfx(sfxName), position);
    }

    private AudioClip GetSfx(string sfxName)
    {
        for (int i = 0; i < soundEffects.Length; i++)
        {
            if (soundEffects[i].name.Equals(sfxName))
            {
                return soundEffects[i];
            }
        }

        Debug.LogError("Super Foo!");
        return null;
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
