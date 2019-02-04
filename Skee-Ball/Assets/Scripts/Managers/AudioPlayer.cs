using UnityEngine;

public class AudioPlayer : Singelton<AudioPlayer>
{
    public MusicTrack[] MusicTracks;
    public Narration[] Narrations;
    public Sfx[] SoundEffects;

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

    public void PlayNarration(int sceneIndex)
    {
        Narrations[sceneIndex].PlayNarration();
    }

    public void StopNarration(int sceneIndex)
    {     
        Narrations[sceneIndex].StopNarration();
    }

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
