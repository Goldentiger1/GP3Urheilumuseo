using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : SingeltonPersistant<GameMaster>
{
    private int currentSceneIndex;

    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void ChangeScene(int sceneIndex)
    {
        StartCoroutine(ILoadSceneAsync(sceneIndex));
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

                if (Input.anyKeyDown)
                {
                    asyncOperation.allowSceneActivation = true;
                }
            }

            yield return null;
        }

        UIManager.Instance.FadeScreenImage(-50f, 2f);
    }
}
