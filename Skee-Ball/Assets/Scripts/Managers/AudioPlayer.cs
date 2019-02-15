using UnityEngine;

public class AudioPlayer : Singelton<AudioPlayer>
{
    public MusicTrack[] MusicTracks;
    public Narration[] Narrations;
    public Sfx[] SoundEffects;

    public string CurrentNarrationClipName { get; set; }

    private void Start()
    {
        CreateMusicTrackAudioSources();
        CreateNarrationAudioSources();
    }

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
        

        MusicTracks[sceneIndex].PlayTrack();
    }

    public void StopMusicTrack(int sceneIndex)
    {
        MusicTracks[sceneIndex].StopTrack();
    }

    #region INDEXIT !!!
    public void PlayNarration(int sceneIndex)
    {
        if (sceneIndex < 1)
            return;

        Narrations[sceneIndex - 1].PlayNarration();
    }

    public void StopNarration(int sceneIndex)
    {
        if (sceneIndex< 1)
            return;

        Narrations[sceneIndex - 1].StopNarration();
    }
    #endregion INDEXIT !!!

    public void PlaySfx(
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
            audioSource.PlayOneShot(GetSoundEffect(AudioClipName));
        }
    }

    public void PlayClipAtPoint(string clipName, Vector3 position, float volume = 1f) 
    {
        AudioSource.PlayClipAtPoint(GetSoundEffect(clipName), position, volume);
    }

    public void PlayLoopingSfx(AudioSource audioSource, string AudioClipName, float volume = 1f, float pitch = 1f)
    {
        if (!audioSource.isPlaying) 
        {
            audioSource.loop = true;
            audioSource.volume = volume;
            audioSource.pitch = pitch;
            audioSource.clip = GetSoundEffect(AudioClipName);
            audioSource.Play();
        }
    }

    private AudioClip GetSoundEffect(string clipName)
    {
        for (int i = 0; i < SoundEffects.Length; i++)
        {
            if(clipName == SoundEffects[i].Name)
            {
                return SoundEffects[i].audioClip;
            }
        }

        return null;
    }
}
