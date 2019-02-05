using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : SingeltonPersistant<GameMaster>
{
    private Coroutine loadSceneAsync;
    public float SceneChangeTimer = 60f;
    public float FakeLoadDuration = 0f;

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

        UIManager.Instance.FadeScreenImage(1f);

        AudioManager.Instance.FadeChannelVolume("Music", 0, 1);
        AudioPlayer.Instance.StopMusicTrack(CurrentSceneIndex);
        AudioPlayer.Instance.StopNarration(CurrentSceneIndex);

        yield return new WaitWhile(() => AudioManager.Instance.IsAudioFading);

        yield return new WaitWhile(() => UIManager.Instance.IsFading);

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

        LocalizationManager.Instance.ChangeTextToNewLanguage();    

        OnSceneChanged();
    }

    private void OnSceneChanged()
    {
        UIManager.Instance.FadeScreenImage(0f);

        AudioManager.Instance.FadeChannelVolume("Music", 1, 1);
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
}
