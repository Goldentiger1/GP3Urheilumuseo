using System.Collections;
using UnityEngine;
using UnityEditor;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class UIManager : Singelton<UIManager>
{
    #region VARIABLES

    private MenuPanel menuPanel;
    private TutorialPanel tutorialPanel;
    private NarrationPanel narrationPanel;

    private UI_Panel previousPanel;
    private UI_Panel currentPanel;

    private readonly float yOffset = 1f;
    private readonly float zOffset = .6f;
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

    public bool IsOptionsConfirmed
    {
        get;
        private set;
    }

    #endregion PROPERTIES

    #region UNITY_FUNCTIONS

    private void Awake()
    {
        HUDCanvas = transform.Find("HUDCanvas");

        menuPanel = GetComponentInChildren<MenuPanel>(includeInactive:true);
        tutorialPanel = GetComponentInChildren<TutorialPanel>(includeInactive: true);
        narrationPanel = GetComponentInChildren<NarrationPanel>(includeInactive: true);

        menuPanel.gameObject.SetActive(false);
        tutorialPanel.gameObject.SetActive(false);
        narrationPanel.gameObject.SetActive(false);

        HUDCanvas.gameObject.SetActive(false);
    }

    private void Start()
    {
        audioFadeInDuration = FadeInDuration;
        audioFadeOutDuration = FadeOutDuration;    

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

    private void SwitchPanel(UI_Panel newPanel)
    {
        if (iShowHUD_Coroutine == null)
        {
            ShowHUD(Player.instance.hmdTransform.position + Vector3.forward, 1f, 400f);
        }

        previousPanel = currentPanel;

        if (previousPanel != null)
            previousPanel.gameObject.SetActive(false);

        currentPanel = newPanel;
        newPanel.gameObject.SetActive(true);
        newPanel.Open();

    }

    public void ShowMenuPanel()
    {
        SwitchPanel(menuPanel);
    }

    public void ShowTutorialPanel(int tutorialTextNumber)
    {
        SwitchPanel(tutorialPanel);

        tutorialPanel.ShowTutorialText(tutorialTextNumber);
    }

    public void ShowNarrationPanel(string key)
    {
        SwitchPanel(narrationPanel);

        narrationPanel.ShowPanel(key);
    }

    private void ShowHUD(Vector3 startPosition,  float showDelay = 0f, float showDuration = 20f)
    {
        iShowHUD_Coroutine = StartCoroutine(
            IShowHUD(
            startPosition,
            showDelay,
            showDuration,
            Player.instance.hmdTransform
            ));       
    }

    public void HideHUD()
    {
        currentPanel.gameObject.SetActive(false);

        if (iShowHUD_Coroutine != null)
        {
            StopCoroutine(iShowHUD_Coroutine);
            iShowHUD_Coroutine = null;
        }

        HUDCanvas.gameObject.SetActive(false);
        IsOptionsConfirmed = false; 
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

    private void OpenUIPanel(UI_Panel uI_Panel)
    {
        uI_Panel.gameObject.SetActive(true);

        uI_Panel.Open();
    }

    private void CloseUIPanel(UI_Panel uI_Panel)
    {
        uI_Panel.Close();

        uI_Panel.gameObject.SetActive(false);
    }

    private IEnumerator IShowHUD(Vector3 startPosition, float showDelay, float showDuration, Transform target)
    {
        yield return new WaitForSeconds(showDelay);

        HUDCanvas.position = startPosition + Vector3.forward;
        HUDCanvas.gameObject.SetActive(true);
    
        AudioPlayer.Instance.PlayClipAtPoint(1, "UIPanelOpen", HUDCanvas.position);

        while(showDuration > 0)
        {
            showDuration -= Time.deltaTime;

            MoveHUD(target);
            RotateHUD(target);

            yield return null;
        }

        HideHUD();
    }

    #region Buttons

    public void QuitButton()
    {
        SteamFadeScreen(FadeColor, 0);
        SteamFadeScreen(FadeColor, FadeOutDuration);

        Invoke("OnQuit", FadeInDuration + 0.2f);
    }

    public void ChangeLanguage(int NEW_LANGUAGE)
    {
        LocalizationManager.Instance.ChangeLanguage((LANGUAGE)NEW_LANGUAGE);
    }

    public void ConfirmOptionsButton()
    {
        if (IsOptionsConfirmed == false)
        {
            IsOptionsConfirmed = true;
        }
    }

    #endregion Buttons

    #endregion CUSTOM_FUNCTIONS
}
