using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : Singelton<AudioPlayer>
{
    #region VARIABLES

    public float NarrationDelay = 0f;

    public MusicTrack[] MusicTracks;
    public Narration[] Narrations;

    public Sfx[] BasketballEffects;
    public Sfx[] UIEffects;
    public Sfx[] OtherEffects;

    private Dictionary<int, Sfx[]> soundEffects = new Dictionary<int, Sfx[]>();

    #endregion VARIABLES

    #region PROPERTIES

    public bool IsNarrationPlaying
    {
        get
        {
            return Narrations[SceneManager.Instance.CurrentScene.Index].IsPlaying;
        }
    }

    #endregion PROPERTIES

    #region UNITY_FUNCTIONS

    private void Start()
    {
        soundEffects.Add(0, BasketballEffects);
        soundEffects.Add(1, UIEffects);
        soundEffects.Add(2, OtherEffects);

        CreateMusicTrackAudioSources();
        CreateNarrationAudioSources();
    }

    #endregion UNITY_FUNCTIONS

    #region CUSTOM_FUNCTIONS

    private void CreateMusicTrackAudioSources()
    {
        for (int i = 0; i < MusicTracks.Length; i++)
        {
            GameObject newGameObject = new GameObject("Music_" + i + "_" + MusicTracks[i].Name);
            newGameObject.transform.SetParent(transform.GetChild(0));
            MusicTracks[i].SetAudioSource(newGameObject.AddComponent<AudioSource>());
        }
    }

    private void CreateNarrationAudioSources()
    {
        for (int i = 0; i < Narrations.Length; i++)
        {
            GameObject newGameObject = new GameObject("Narration_" + i + "_" + Narrations[i].Name);
            newGameObject.transform.SetParent(transform.GetChild(1));
            Narrations[i].SetAudioSource(newGameObject.AddComponent<AudioSource>());
        }
    }

    public void PlayMusicTrack(int sceneIndex)
    {
        GetMusicTrack(sceneIndex).PlayTrack();
    }

    public void StopMusicTrack(int sceneIndex)
    {
        GetMusicTrack(sceneIndex).StopTrack();
    }

    public void PlayNarration(int sceneIndex)
    {
        StartCoroutine(IPlayNarration(sceneIndex, NarrationDelay));
    }

    public void StopNarration()
    {
        for (int i = 0; i < Narrations.Length; i++)
        {
            Narrations[i].StopNarration();
        }
    }

    public void PlaySfx(
        int sfxIndex,
        AudioSource audioSource,
        string AudioClipName,
        float minRandomVolume = 0.9f,
        float maxRandomVolume = 1.1f,
        float minRandomPitch = 0.9f,
        float maxRandomPitch = 1.1f)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.volume = Random.Range(minRandomVolume, maxRandomVolume);
            audioSource.pitch = Random.Range(minRandomPitch, maxRandomPitch);
            audioSource.PlayOneShot(GetSoundEffect(sfxIndex, AudioClipName));
        }
    }

    public void PlayClipAtPoint(int sfxIndex, string clipName, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(GetSoundEffect(sfxIndex, clipName), position, volume);
    }

    public void PlayLoopingSfx(int sfxIndex, AudioSource audioSource, string AudioClipName, float volume = 1f, float pitch = 1f)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.loop = true;
            audioSource.volume = volume;
            audioSource.pitch = pitch;
            audioSource.clip = GetSoundEffect(sfxIndex, AudioClipName);
            audioSource.Play();
        }
    }

    private MusicTrack GetMusicTrack(int sceneIndex)
    {
        switch (sceneIndex)
        {
            case 0:
                return MusicTracks[0];
            case 4:
                return MusicTracks[2];
            default:
                return MusicTracks[1];
        }
    }

    public AudioClip GetSoundEffect(int sfxIndex, string clipName)
    {
        for (int i = 0; i < soundEffects[sfxIndex].Length; i++)
        {
            if (clipName == soundEffects[sfxIndex][i].Name)
            {
                return soundEffects[sfxIndex][i].audioClip;
            }
        }

        return null;
    }

    private IEnumerator IPlayNarration(int sceneIndex, float delay)
    {
        yield return new WaitForSeconds(delay);

        Narrations[sceneIndex - 1].PlayNarration();
    }

    #endregion CUSTOM_FUNCTIONS
}
