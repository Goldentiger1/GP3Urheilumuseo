using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : SingeltonPersistant<GameMaster>
{
    private Coroutine loadSceneAsync;
    [SerializeField]
    private float sceneChangeTimer = 60f;
    [SerializeField]
    private float fakeLoadDuration = 0f;

    private bool isChangingScene;
    private int sceneCount;

    public int CurrentSceneIndex
    {
        get;
        private set;
    }

    private void Start()
    {
        sceneCount = SceneManager.sceneCountInBuildSettings;
        CurrentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        OnSceneChanged();
    }

    public void ChangeScene(int sceneIndex, float sceneChangeTimer = 0f)
    {
        if(loadSceneAsync == null)
        {
            loadSceneAsync = StartCoroutine(ILoadSceneAsync(sceneIndex, sceneChangeTimer));
        }    
    }

    public void RestartScene()
    {
        ChangeScene(CurrentSceneIndex);
    }

    private IEnumerator ILoadSceneAsync(int sceneIndex, float sceneChangeTimer)
    {
        yield return new WaitForSeconds(sceneChangeTimer);

        LevelManager.Instance.ClearBasketBalls();
        isChangingScene = true;

        //UIManager.Instance.FadeScreenImage(1f);

        //yield return new WaitWhile(() => UIManager.Instance.IsFading);

        var asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress == 0.9f)
            {
                LocalizationManager.Instance.ClearLocalizedText();

                yield return new WaitForSeconds(fakeLoadDuration);
                
                CurrentSceneIndex = sceneIndex;
                asyncOperation.allowSceneActivation = true;
                
            }

            yield return null;
        }

        loadSceneAsync = null;

        isChangingScene = false;

        LocalizationManager.Instance.ChangeTextToNewLanguage();

        OnSceneChanged();
    }

    private void OnSceneChanged()
    {
        //UIManager.Instance.FadeScreenImage(0f);
        AudioManager.Instance.ChangeMusicTrack(CurrentSceneIndex);

        if(CurrentSceneIndex == 0)
        {
            return;
        }

        if(CurrentSceneIndex == sceneCount - 1)
        {
            ChangeScene(0, sceneChangeTimer);
            return;
        }

        ChangeScene(CurrentSceneIndex + 1, sceneChangeTimer);
    }
}
