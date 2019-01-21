using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelController : MonoBehaviour
{
    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void ChangeLanguage(string language)
    {
        LocalizationManager.Instance.ChangeLanguage(language);
    }
}
