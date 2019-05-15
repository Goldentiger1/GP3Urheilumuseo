using TMPro;
using UnityEngine;

public class LocalizedText : MonoBehaviour
{
    public string Key;

    private TextMeshProUGUI textMeshProUGUI;
    private TextMeshPro textMeshPro;

    public string Text
    {
        get
        {
            if (textMeshProUGUI != null)
            {
                return textMeshProUGUI.text;
            }
            else if (textMeshPro != null)
            {
                return textMeshPro.text;
            }
            else
            {
                return "Super Foo!";
            }
        }
        set
        {
            if(textMeshProUGUI != null)
            {
                textMeshProUGUI.text = value;
            }
            else if (textMeshPro != null)
            {
                textMeshPro.text = value;
            }
            else
            {
                Debug.LogError("Super Foo!");
            }
        }
    }

    private void Awake()
    {     
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        textMeshPro = GetComponent<TextMeshPro>();
    }

    private void OnEnable()
    {
        LocalizationManager.Instance.AddLocalizedText(this);
    }
}
