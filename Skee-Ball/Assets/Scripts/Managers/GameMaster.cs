using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : SingeltonPersistant<GameMaster>
{
    private Coroutine loadSceneAsync;

    private readonly float fakeLoadDuration = 4f;

    private int currentSceneIndex;

    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void ChangeScene(int sceneIndex)
    {
        if(loadSceneAsync == null)
        loadSceneAsync = StartCoroutine(ILoadSceneAsync(sceneIndex));
    }

    private IEnumerator ILoadSceneAsync(int sceneIndex)
    {
        UIManager.Instance.FadeScreenImage(10f, 2f);

        yield return new WaitWhile(() => UIManager.Instance.IsFading);

        var asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress == 0.9f)
            {
                LocalizationManager.Instance.ClearLocalizedText();

                yield return new WaitForSeconds(fakeLoadDuration);
                
                currentSceneIndex = sceneIndex;
                asyncOperation.allowSceneActivation = true;
                
            }

            yield return null;
        }

        UIManager.Instance.FadeScreenImage(-50f, 2f);
    }
}
