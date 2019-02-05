using UnityEngine;
using TMPro;

public class UIManager : Singelton<UIManager>
{
    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        
    }

    private void Initialize()
    {
        
    }


    //public void FadeScreenImage(float targetFillAmount, float fadeSpeed = 1f, float startFadeDelay = 0f)
    //{
    //    StartCoroutine(IFadeScreenImage(targetFillAmount, fadeSpeed, startFadeDelay));
    //}

    //private IEnumerator IFadeScreenImage(float targetFillAmount, float fadeSpeed, float startFadeDelay)
    //{
    //    IsFading = true;
    //    canvasGroup.blocksRaycasts = true;
    //    LoadImage.gameObject.SetActive(false);

    //    targetFillAmount = targetFillAmount > 1f ? 1f : targetFillAmount;
    //    targetFillAmount = targetFillAmount < 0f ? 0f : targetFillAmount;

    //    yield return new WaitForSeconds(startFadeDelay > 0f ? startFadeDelay : 0f);

    //    while (canvasGroup.alpha != targetFillAmount) // ???
    //    {
    //        canvasGroup.alpha += canvasGroup.alpha < targetFillAmount ? (1f / fadeSpeed) * Time.unscaledDeltaTime : -(1f / fadeSpeed) * Time.unscaledDeltaTime;
    //        yield return null;
    //    }

    //    LoadImage.gameObject.SetActive(true);
    //    canvasGroup.blocksRaycasts = false;
    //    IsFading = false;
    //}

    public void UpdateScoreVisuals(string newScoreText)
    {
               
        //uiScoreText.text = "SCORE 0" + newScoreText;
                    
        //uiScoreText.text = "SCORE " + newScoreText;
                                    
    }
}
