using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;

public class GameMaster : SingeltonPersistant<GameMaster>
{
    private Coroutine loadSceneAsync;

    [Header("Scene load variables")]
    [Range(0, 200)]
    public float SceneChangeTimer = 60f;
    [Range(0, 20)]
    public float FakeLoadDuration = 0f;

    [Header("Fade variables")]
    [Range(0, 10)]
    public float FadeInDuration = 0f;
    [Range(0, 10)]
    public float FadeOutDuration = 0f;

    public Color FadeColor = Color.black;

    private float audioFadeInDuration;
    private float audioFadeOutDuration;

    private int sceneCount;

    public int CurrentSceneIndex
    {
        get;
        private set;
    }
    public int NextSceneIndex
    {
        get
        {
            return CurrentSceneIndex + 1;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        Initialize();
    }

    private void Start()
    {      
        OnSceneChanged();
    }

    private void Initialize()
    {
        sceneCount = SceneManager.sceneCountInBuildSettings;
        CurrentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        audioFadeInDuration = FadeInDuration;
        audioFadeOutDuration = FadeOutDuration;
    }

    public void ChangeScene(int sceneIndex, float sceneChangeTimer = 0f)
    {
        if(loadSceneAsync == null)
        {
            loadSceneAsync = StartCoroutine(ILoadSceneAsync(sceneIndex, sceneChangeTimer));
        }    
    }

    public void ChangeNextScene()
    {
        ChangeScene(NextSceneIndex);
    }

    public void RestartScene()
    {
        ChangeScene(CurrentSceneIndex);
    }

    private IEnumerator ILoadSceneAsync(int sceneIndex, float sceneChangeTimer)
    {
        yield return new WaitForSeconds(sceneChangeTimer);

        LevelManager.Instance.ClearBasketBalls();

        SteamFadeScreen(FadeColor, FadeInDuration);

        AudioManager.Instance.FadeChannelVolume("Music", 0, audioFadeOutDuration);
       
        yield return new WaitWhile(() => AudioManager.Instance.IsAudioFading);

        AudioPlayer.Instance.StopMusicTrack(CurrentSceneIndex);
        AudioPlayer.Instance.StopNarration(CurrentSceneIndex);

        Debug.LogError(SteamVR_Fade.Instance.IsFading);
        yield return new WaitWhile(() => SteamVR_Fade.Instance.IsFading);
        Debug.LogError(SteamVR_Fade.Instance.IsFading);

        var asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {

            if (asyncOperation.progress == 0.9f)
            {
                LocalizationManager.Instance.ClearLocalizedText();

                yield return new WaitForSeconds(FakeLoadDuration);
                
                CurrentSceneIndex = sceneIndex;
                asyncOperation.allowSceneActivation = true;                
            }

            yield return null;
        }

        loadSceneAsync = null;

        OnSceneChanged();
    }

    private void OnSceneChanged()
    {
        LocalizationManager.Instance.ChangeTextToNewLanguage();
        LevelManager.Instance.ResetScores();

        SteamFadeScreen(FadeColor, 0);
        SteamFadeScreen(Color.clear, FadeOutDuration);

        AudioManager.Instance.FadeChannelVolume("Music", 1, audioFadeInDuration);
        AudioPlayer.Instance.PlayMusicTrack(CurrentSceneIndex);

        if (CurrentSceneIndex == 0)
        {
            return;
        }

        AudioPlayer.Instance.PlayNarration(CurrentSceneIndex);

        if (CurrentSceneIndex == sceneCount - 1)
        {
            ChangeScene(0, SceneChangeTimer);
            return;
        }

        ChangeScene(NextSceneIndex, SceneChangeTimer);
    }

    private void SteamFadeScreen(Color color, float fadeDuration)
    {
        SteamVR_Fade.Start(color, fadeDuration, true);
    }
}
