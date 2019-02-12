using TMPro;
using UnityEngine;

public class LocalizedText : MonoBehaviour
{
    public string Key;

    private TextMeshProUGUI textMeshProUGUI;
    private TextMeshPro textMeshPro;

    public string Text
    {
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

        LocalizationManager.Instance.AddLocalizedText(this);
    }
}
