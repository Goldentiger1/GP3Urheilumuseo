using System.Collections;
using UnityEngine;

public class UIManager : Singelton<UIManager>
{
    private Transform hudCanvas;
    private CanvasGroup canvasGroup;

    public bool IsFading
    {
        get;
        private set;
    }

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        canvasGroup.alpha = 1f;

        FadeScreenImage(0f, 2f);
    }

    private void Initialize()
    {
        hudCanvas = transform.Find("HudCanvas");
        canvasGroup = hudCanvas.GetComponent<CanvasGroup>();
    }

    public void FadeScreenImage(float targetFillAmount, float fadeSpeed = 1f, float startDelay = 0f)
    {
        StartCoroutine(IFadeScreenImage(targetFillAmount, fadeSpeed, startDelay));
    }

    private IEnumerator IFadeScreenImage(float targetFillAmount, float fadeSpeed, float startFadeDelay)
    {
        IsFading = true;
        canvasGroup.blocksRaycasts = true;

        targetFillAmount = targetFillAmount > 1f ? 1f : targetFillAmount;
        targetFillAmount = targetFillAmount < 0f ? 0f : targetFillAmount;

        yield return new WaitForSeconds(startFadeDelay > 0f ? startFadeDelay : 0f);

        while (canvasGroup.alpha != targetFillAmount)
        {
            canvasGroup.alpha += canvasGroup.alpha < targetFillAmount ? (1f / fadeSpeed) * Time.unscaledDeltaTime : -(1f / fadeSpeed) * Time.unscaledDeltaTime;
            yield return null;
        }

        canvasGroup.blocksRaycasts = false;
        IsFading = false;
    }
}
