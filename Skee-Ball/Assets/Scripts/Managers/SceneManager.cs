﻿using System;
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

    private void ChangeScene(int sceneIndex)
    {
        if (loadSceneAsync == null)
        {
            loadSceneAsync = StartCoroutine(ILoadSceneAsync(sceneIndex));
        }
    }

    public void ChangeNextScene()
    {
        //if (IsFirstScene)
        //{
        //    return;
        //}

        if (IsLastScene)
        {
            ChangeScene(0);
            return;
        }

        ChangeScene(NextScene.Index);
    }

    public void RestartScene()
    {
        ChangeScene(CurrentScene.Index);
    }

    private IEnumerator ILoadSceneAsync(int sceneIndex)
    {
        //!!??
        yield return new WaitWhile(() => AudioPlayer.Instance.IsNarrationPlaying);

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
        LevelManager.Instance.ResetLevelValues();

        UIManager.Instance.FadeScreenOut();

        AudioPlayer.Instance.PlayMusicTrack(CurrentScene.Index);
        AudioPlayer.Instance.PlayNarration(CurrentScene.Index);

        LocalizationManager.Instance.ChangeTextToNewLanguage();     
    }
}