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

    private void Start()
    {
        LocalizationManager.Instance.ChangeTextToNewLanguage();
    }

    public void ShowPanel(string key)
    {
        narrationText.Key = key;

        textMeshPro.text = LocalizationManager.Instance.GetValue(key);

        animator.SetBool("IsShowing", true);

    }

    public void ClosePanel()
    {
        animator.SetBool("IsShowing", false);
    }
}
