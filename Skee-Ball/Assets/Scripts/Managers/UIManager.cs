using System.Collections;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class UIManager : Singelton<UIManager>
{
    private readonly float zOffset = 2f;
    private float startYPosition;

    #region VARIABLES

    private Coroutine iShowHUD;

    private Transform HUDCanvas;
    private float audioFadeInDuration;
    private float audioFadeOutDuration;

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
    }

    private void Update()
    {
        if (HUDCanvas.gameObject.activeSelf == false)
            return;

        MoveHUD();
    }

    private void Start()
    {
        audioFadeInDuration = FadeInDuration;
        audioFadeOutDuration = FadeOutDuration;

        startYPosition = transform.position.y + 1;

        HUDCanvas.gameObject.SetActive(false);

        SteamFadeScreen(FadeColor, 0);
    }

    #endregion UNITY_FUNCTIONS

    #region CUSTOM_FUNCTIONS

    private void MoveHUD()
    {
        var playerDirection = Player.instance.headCollider.transform;
        //Vector3 desiredPosition = playerDirection.position + playerDirection.forward * zOffset;
        //HUDCanvas.position = new Vector3(Mathf.Lerp(HUDCanvas.position.x, desiredPosition.x, Time.deltaTime), startYPosition, desiredPosition.z);

        Vector3 desiredPosition = playerDirection.position + playerDirection.forward * zOffset;
        HUDCanvas.position = desiredPosition;

        HUDCanvas.transform.LookAt(playerDirection);
        HUDCanvas.transform.position = new Vector3(playerDirection.position.x, startYPosition, playerDirection.position.z);
    }

    public void ShowHUD(float showDuration = 20f)
    {
        if(iShowHUD == null)
        {
            iShowHUD = StartCoroutine(IShowHUD(showDuration));
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

    private IEnumerator IShowHUD(float showDuration)
    {
        HUDCanvas.gameObject.SetActive(true);

        yield return new WaitForSeconds(showDuration);

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
