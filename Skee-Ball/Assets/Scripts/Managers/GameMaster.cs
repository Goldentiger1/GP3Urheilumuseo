using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : SingeltonPersistant<GameMaster>
{
    private Coroutine loadSceneAsync;

    private readonly float fakeLoadDuration = 4f;

    public int CurrentSceneIndex
    {
        get;
        private set;
    }
    public bool IsChangingScene
    {
        get;
        private set;
    }

    private void Start()
    {
        CurrentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        OnSceneChanged();
    }

    public void ChangeScene(int sceneIndex)
    {
        if(loadSceneAsync == null)
        loadSceneAsync = StartCoroutine(ILoadSceneAsync(sceneIndex));
    }

    public void RestartScene()
    {
        ChangeScene(CurrentSceneIndex);
    }

    private IEnumerator ILoadSceneAsync(int sceneIndex)
    {
        IsChangingScene = true;

        UIManager.Instance.FadeScreenImage(1f, 2f);

        yield return new WaitWhile(() => UIManager.Instance.IsFading);

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

        IsChangingScene = false;

        LocalizationManager.Instance.ChangeTextToNewLanguage();

        OnSceneChanged();
    }

    private void OnSceneChanged()
    {
        UIManager.Instance.FadeScreenImage(0f, 2f);
        AudioManager.Instance.ChangeMusicTrack(CurrentSceneIndex);
    }
}
