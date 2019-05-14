using System.Collections;
using UnityEngine;
using UnityEditor;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class UIManager : Singelton<UIManager>
{
    #region VARIABLES

    public UI_Panel[] UI_Panels;

    private readonly float yOffset = 1f;
    private readonly float zOffset = 1.2f;
    private readonly float smoothMultiplier = 0.6f;

    private Coroutine iShowHUD_Coroutine;

    private Transform HUDCanvas;
    private float audioFadeInDuration;
    private float audioFadeOutDuration;

    [Header("Fade Variables")]
    [Range(0, 10)]
    public float FadeInDuration = 0f;
    [Range(0, 10)]
    public float FadeOutDuration = 0f;
    public Color FadeColor = Color.black;

    #endregion VARIABLES

    #region PROPERTIES



    #endregion PROPERTIES

    #region UNITY_FUNCTIONS

    private void Awake()
    {
        HUDCanvas = transform.Find("HUDCanvas");

        for (int i = 0; i < UI_Panels.Length; i++)
        {
            UI_Panels[i].gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        audioFadeInDuration = FadeInDuration;
        audioFadeOutDuration = FadeOutDuration;

        HUDCanvas.gameObject.SetActive(false);

        // !!!!!!!!!
        HUDCanvas.transform.localScale = new Vector3(-1, 1, 1);

        SteamFadeScreen(FadeColor, 0);
    }

    #endregion UNITY_FUNCTIONS

    #region CUSTOM_FUNCTIONS

    private void MoveHUD(Transform target)
    {
        var desiredPosition = new Vector3(
            target.position.x,
            HUDCanvas.position.y,
            target.position.z
            );

        HUDCanvas.position = Vector3.Lerp(
            HUDCanvas.position, 
            desiredPosition + (target.forward * zOffset), 
            Time.deltaTime * smoothMultiplier);

        HUDCanvas.position = new Vector3(
            HUDCanvas.position.x,
            yOffset,
            HUDCanvas.position.z
            );
    }

    private void RotateHUD(Transform target)
    {
        HUDCanvas.LookAt(
            new Vector3(target.position.x,
            HUDCanvas.position.y, 
            target.position.z)
            );
    }

    /// <summary>
    ///  uiPanelIndex = 0 => MenuPanel
    ///  uiPanelIndex = 1 => TutorialPanel
    ///  uiPanelIndex = 2 => NarrationPanel
    /// </summary>
    /// <param name="uiPanelIndex"></param>
    /// <param name="startPosition"></param>
    /// <param name="showDelay"></param>
    /// <param name="showDuration"></param>
    public void ShowHUD(int uiPanelIndex, Vector3 startPosition,  float showDelay = 0f, float showDuration = 20f)
    {
        if (iShowHUD_Coroutine == null)
        {
            iShowHUD_Coroutine = StartCoroutine(
                IShowHUD(uiPanelIndex,
                startPosition,
                showDelay,
                showDuration,
                Player.instance.hmdTransform
                ));
        }
    }

    public void HideHUD()
    {
        if(iShowHUD_Coroutine != null)
        {
            StopCoroutine(iShowHUD_Coroutine);
            iShowHUD_Coroutine = null;
        }

        AudioPlayer.Instance.PlayClipAtPoint(1 ,"UIPanelClose", HUDCanvas.position);

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
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private IEnumerator IShowHUD(int uiPanelIndex ,Vector3 startPosition, float showDelay, float showDuration, Transform target)
    {
        yield return new WaitForSeconds(showDelay);

        HUDCanvas.position = startPosition + Vector3.forward;
        HUDCanvas.gameObject.SetActive(true);

        OpenUIPanel(UI_Panels[uiPanelIndex]);
       
        AudioPlayer.Instance.PlayClipAtPoint(1, "UIPanelOpen", HUDCanvas.position);

        while(showDuration > 0)
        {
            showDuration -= Time.deltaTime;

            MoveHUD(target);
            RotateHUD(target);

            yield return null;
        }

        HideHUD();
        iShowHUD_Coroutine = null;
    }

    #region Buttons

    public void QuitButton()
    {
        SteamFadeScreen(FadeColor, 0);
        SteamFadeScreen(FadeColor, FadeOutDuration);

        Invoke("OnQuit", FadeInDuration + 0.2f);
    }

    #endregion Buttons

    #endregion CUSTOM_FUNCTIONS


    private void OpenUIPanel(UI_Panel uI_Panel)
    {
        uI_Panel.gameObject.SetActive(true);

        uI_Panel.OpenPanel();
    }

    private void CloseUIPanel(UI_Panel uI_Panel)
    {
        uI_Panel.ClosePanel();

        uI_Panel.gameObject.SetActive(false);      
    }
}
