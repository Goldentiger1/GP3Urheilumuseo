using TMPro;
using UnityEngine;

public class NarrationPanel : Singelton<NarrationPanel>
{
    private LocalizedText narrationText;
    private TextMeshPro textMeshPro;

    private Animator animator;

    private void Awake()
    {
        narrationText = GetComponent<LocalizedText>();
        textMeshPro = GetComponent<TextMeshPro>();
        animator = GetComponent<Animator>();       
    }

    public void ShowPanel(string key)
    {
        narrationText.Key = key;

        LocalizationManager.Instance.ChangeTextToNewLanguage();

        animator.SetBool("IsShowing", true);

    }

    public void ClosePanel()
    {
        animator.SetBool("IsShowing", false);
    }
}
