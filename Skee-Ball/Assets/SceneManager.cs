using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;

[Serializable]
public class SceneData
{
    public int Index { get; private set; }
    public string Name { get; private set; }

    public SceneData(int index)
    {
        Index = index;
        Name = GetSceneName(Index);
    }

    private string GetSceneName(int index)
    {
        var path = SceneUtility.GetScenePathByBuildIndex(index);
        var slash = path.LastIndexOf('/');
        var name = path.Substring(slash + 1);
        var dot = name.LastIndexOf('.');

        return name.Substring(0, dot);
    }
}

public class SceneManager : Singelton<SceneManager>
{
    private SceneData[] gameScenes;

    private Coroutine loadSceneAsync;

    [Header("Scene load variables")]
    [Range(0, 200)]
    public float SceneChangeTimer = 60f;
    [Range(0, 20)]
    public float FakeLoadDuration = 0f;

    public SceneData CurrentScene
    {
        get
        {
            return gameScenes[UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex];
        }
    }
    public SceneData NextScene
    {
        get
        {
            return gameScenes[UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1];
        }
    }
    public int SceneCount
    {
        get
        {
            return UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
        }
    }
    public bool IsFirstScene
    {
        get
        {
            return CurrentScene == gameScenes[0];
        }
    }
    public bool IsLastScene
    {
        get
        {
            return CurrentScene == gameScenes[gameScenes.Length - 1];
        }
    }

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        OnSceneChanged();
    }

    private void Initialize()
    {
        gameScenes = new SceneData[SceneCount];

        for (int i = 0; i < gameScenes.Length; i++)
        {
            gameScenes[i] = new SceneData(i);
        }
    }

    public void ChangeScene(int sceneIndex, float sceneChangeTimer = 0f)
    {
        if (loadSceneAsync == null)
        {
            loadSceneAsync = StartCoroutine(ILoadSceneAsync(sceneIndex, sceneChangeTimer));
        }
    }

    public void ChangeNextScene(float sceneChangeTimer = 0f)
    {
        ChangeScene(NextScene.Index, sceneChangeTimer);
    }

    public void RestartScene()
    {
        ChangeScene(CurrentScene.Index);
    }

    private IEnumerator ILoadSceneAsync(int sceneIndex, float sceneChangeTimer)
    {
        yield return new WaitForSeconds(sceneChangeTimer);

        LevelManager.Instance.ClearBasketBalls();

        UIManager.Instance.FadeScreenIn();

        yield return new WaitWhile(() => SteamVR_Fade.IsFading);
        yield return new WaitUntil(() => AudioManager.Instance.IsAudioFading == false);

        AudioPlayer.Instance.StopMusicTrack(CurrentScene.Index);
        AudioPlayer.Instance.StopNarration(CurrentScene.Index);

        var asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneIndex);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress == 0.9f)
            {
                LocalizationManager.Instance.ClearLocalizedText();
                yield return new WaitForSeconds(FakeLoadDuration);
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }

        loadSceneAsync = null;

        OnSceneChanged();
    }

    private void OnSceneChanged()
    {       
        LevelManager.Instance.ResetScores();

        UIManager.Instance.FadeScreenOut();

        AudioPlayer.Instance.PlayMusicTrack(CurrentScene.Index);
        AudioPlayer.Instance.PlayNarration(CurrentScene.Index);

        LocalizationManager.Instance.ChangeTextToNewLanguage();

        if (IsFirstScene)
        {
            return;
        }

        if (IsLastScene)
        {
            ChangeScene(0, SceneChangeTimer);
            return;
        }

        ChangeScene(NextScene.Index, SceneChangeTimer);
    }
}
