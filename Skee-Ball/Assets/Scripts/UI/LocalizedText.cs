using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocalizedText : MonoBehaviour
{
    public string Key;

    private Text text;

    public string Text
    {
        set
        {
            text.text = value;
        }
    }

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void Start()
    {
        LocalizationManager.Instance.AddLocalizedText(this);
    }
}
