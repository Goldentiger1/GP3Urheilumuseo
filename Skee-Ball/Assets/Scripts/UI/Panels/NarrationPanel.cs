using TMPro;
using UnityEngine;

public class NarrationPanel : UI_Panel
{
    private LocalizedText localizedText;
    private TextMeshProUGUI narrationText;

    private bool hasInitialized = false;

    private Animator animator;

    private void Awake()
    {
        Initialize();      
    }

    private void Initialize()
    {
        if (hasInitialized)
            return;

        localizedText = GetComponentInChildren<LocalizedText>();
        narrationText = GetComponentInChildren<TextMeshProUGUI>();
        animator = GetComponent<Animator>();
        hasInitialized = true;
    }

    public void ShowPanel(string key)
    {
        Initialize();

        localizedText.Key = key;

        narrationText.text = LocalizationManager.Instance.GetValue(key);

        animator.SetBool("IsShowing", true);

    }

    public override void Close()
    {
        animator.SetBool("IsShowing", false);
    }

    public void SkipNarration()
    {
        AudioPlayer.Instance.StopNarration();
        UIManager.Instance.HideHUD();

        LevelManager.Instance.IsGameStarted = true;
    }
}
