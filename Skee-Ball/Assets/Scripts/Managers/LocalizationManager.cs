using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class LocalizationData
{
    public LocalizationItem[] Items;
}

[Serializable]
public class LocalizationItem
{
    public string Key;
    public string Value;
}

public class LocalizationManager : Singelton<LocalizationManager>
{
    private Dictionary<string, string> localizationText;

    private readonly string defaultLanguage = "FI";
    private readonly string missingText = "Localized text not found!";

    public bool IsReady { get; private set; }

    private void Awake()
    {
        IsReady = false;

        ChangeLanguage(defaultLanguage);
        //ChangeLanguage("UK");
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => IsReady);

        Debug.Log("Localization is ready");

        // kaikki localizedText objectit tulee ajaa ChangeText-funktio.
    }

    private void LoadLocalizedText(string fileName)
    {
        print("LoadLocalizedText");

        localizationText = new Dictionary<string, string>();

        string filePath = Path.Combine(Application.streamingAssetsPath, "LocalizedText_" + fileName + ".json");

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);

            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);

            for (int i = 0; i < loadedData.Items.Length; i++)
            {
                localizationText.Add(loadedData.Items[i].Key, loadedData.Items[i].Value);
            }

            Debug.Log("Data loaded, dictionary contains: " + localizationText.Count + " entries.");
        }
        else
        {
            Debug.LogError("Cannot find file!");
        }

        IsReady = true;
    }

    public void ChangeLanguage(string newLanguage)
    {
        LoadLocalizedText(newLanguage);
    }

    public string GetValue(string key)
    {
        var result = string.Empty;

        return result = localizationText.TryGetValue(key, out result) ? result : missingText;
    }
}
