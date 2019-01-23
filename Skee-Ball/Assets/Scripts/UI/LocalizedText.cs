using System;
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
        LocalizationManager.Instance.RegisterCallback(
            
            (Key, result) => { text.text = result; }

            );
    }
}
