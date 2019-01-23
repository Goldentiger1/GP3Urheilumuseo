using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocalizedText : MonoBehaviour
{
    public string Key;

    private Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void Start()
    {
        // Hei olen olemaassa Localization Manager!
        LocalizationManager.Instance.AddLocalizedText(this);
    }

    public void ChangeText(string newText)
    {
        text.text = newText;
    }
}
