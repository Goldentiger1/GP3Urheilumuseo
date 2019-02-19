using System.Collections;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class UIManager : Singelton<UIManager>
{
    #region VARIABLES
    
    private readonly float yOffset = 0.8f;
    private readonly float zOffset = 1.2f;
    private readonly float smoothMultiplier = 0.6f;

    private Coroutine iShowHUD;

    private Transform HUDCanvas;
    private float audioFadeInDuration;
    private float audioFadeOutDuration;

    private Animator animator;

    [Header("Fade variables")]
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
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        audioFadeInDuration = FadeInDuration;
        audioFadeOutDuration = FadeOutDuration;

        HUDCanvas.gameObject.SetActive(false);

        SteamFadeScreen(FadeColor, 0);
    }

    private void Update()
    {
        if (HUDCanvas.gameObject.activeSelf == false)
            return;

        var target = Player.instance.hmdTransform;

        MoveHUD(target);
        RotateHUD(target);
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

            new Vector3(target.position.x, HUDCanvas.position.y, target.position.z)

            );
    }

    public void ShowHUD(Vector3 startPosition,  float showDelay = 0f, float showDuration = 20f)
    {
        if (iShowHUD == null)
        {
            iShowHUD = StartCoroutine(IShowHUD(startPosition, showDelay, showDuration));
        }
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

    private IEnumerator IShowHUD(Vector3 startPosition, float showDelay, float showDuration)
    {
        yield return new WaitForSeconds(showDelay);

        HUDCanvas.position = startPosition;
        HUDCanvas.gameObject.SetActive(true);

        AudioPlayer.Instance.PlayClipAtPoint("UIPanelOpen", HUDCanvas.position);

        yield return new WaitForSeconds(showDuration);

        AudioPlayer.Instance.PlayClipAtPoint("UIPanelClose", HUDCanvas.position);

        HUDCanvas.gameObject.SetActive(false);

        iShowHUD = null;
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
}
