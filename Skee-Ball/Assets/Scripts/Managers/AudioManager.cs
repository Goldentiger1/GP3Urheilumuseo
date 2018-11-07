using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private AudioMixer audioMixer;
    private AudioClip[] musics;
    private AudioClip[] sfxs;

    private void Awake()
    {
        audioMixer = Resources.Load<AudioMixer>("Audio/Mixers/AudioMixer");
        musics = Resources.LoadAll<AudioClip>("Audio/Music/");
        sfxs = Resources.LoadAll<AudioClip>("Audio/Sfx/");
    }
}
