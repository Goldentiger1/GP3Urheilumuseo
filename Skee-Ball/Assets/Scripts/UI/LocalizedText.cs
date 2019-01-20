using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocalizedText : MonoBehaviour
{
    public string Key;

    private Text text;
    private delegate void Delegate();

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void Start()
    {
        ChangeText();
    }

    public void ChangeText()
    {
        text.text = LocalizationManager.Instance.GetValue(Key);
    }
}
