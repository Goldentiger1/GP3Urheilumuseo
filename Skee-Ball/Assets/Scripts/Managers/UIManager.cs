using UnityEngine;
using Valve.VR;

public class UIManager : Singelton<UIManager>
{
    private Transform HUDCanvas;

    private float audioFadeInDuration;
    private float audioFadeOutDuration;

    [Header("Fade variables")]
    [Range(0, 10)]
    public float FadeInDuration = 0f;
    [Range(0, 10)]
    public float FadeOutDuration = 0f;

    public Color FadeColor = Color.black;

    private void Awake() 
    {
        HUDCanvas = transform.Find("HUDCanvas");
    }

    private void Start() 
    {
        audioFadeInDuration = FadeInDuration;
        audioFadeOutDuration = FadeOutDuration;

        HUDCanvas.gameObject.SetActive(false);

        SteamFadeScreen(FadeColor, 0);
    }

    public void ShowText(float duration) 
    {
        HUDCanvas.gameObject.SetActive(true);

        Invoke("HideCanvas", duration);
    }

    public void ShowLoadingImage(float duration)
    {

    }

    private void HideCanvas() 
    {
        HUDCanvas.gameObject.SetActive(false);
    }

    public void FadeScreenIn()
    {
        SteamFadeScreen(Color.clear, 0);
        SteamFadeScreen(FadeColor, FadeInDuration);

        AudioManager.Instance.FadeChannelVolume("Master", 0, audioFadeOutDuration);
    }

    public void FadeScreenOut()
    {
        SteamFadeScreen(FadeColor, 0);
        SteamFadeScreen(Color.clear, FadeOutDuration);

        AudioManager.Instance.FadeChannelVolume("Master", 1, audioFadeInDuration);
    }

    private void SteamFadeScreen(Color color, float fadeDuration)
    {
        SteamVR_Fade.Start(color, fadeDuration, true);
    }

    private void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    #region Buttons

    public void QuitButton()
    {
        SteamFadeScreen(FadeColor, 0);
        SteamFadeScreen(FadeColor, FadeOutDuration);

        Invoke("OnQuit", FadeInDuration + 0.2f);
    }

    #endregion Buttons
}
