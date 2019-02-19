using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
    #region VARIABLES

    private string currentLanguage = string.Empty;

    private Dictionary<string, string> localizationTextDictionary;
    private List<LocalizedText> localizedTextsInScene = new List<LocalizedText>();

    private readonly string defaultLanguage = "FI";
    private readonly string missingText = "Localized text not found!";

    #endregion VARIABLES

    #region PROPERTIES

    public bool IsReady
    {
        get;
        private set;
    }

    #endregion PROPERTIES

    #region UNITY_FUNCTIONS

    private void Awake()
    {
        IsReady = false;

        ChangeLanguage(defaultLanguage);
    }

    #endregion UNITY_FUNCTIONS

    #region CUSTOM_FUNCTIONS

    private void LoadLocalizedText(string fileName)
    {
        localizationTextDictionary = new Dictionary<string, string>();

        string filePath = Path.Combine(Application.streamingAssetsPath, "LocalizedText_" + fileName + ".json");

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath, Encoding.Default);

            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);

            for (int i = 0; i < loadedData.Items.Length; i++)
            {
                localizationTextDictionary.Add(loadedData.Items[i].Key, loadedData.Items[i].Value);
            }

            //Debug.Log("Data loaded, dictionary contains: " + localizationTextDictionary.Count + " entries.");
        }
        else
        {
            Debug.LogError("Cannot find file!");
        }

        IsReady = true;
    }

    public void AddLocalizedText(LocalizedText newLocalizedText)
    {
        localizedTextsInScene.Add(newLocalizedText);
    }

    public void ClearLocalizedText()
    {
        localizedTextsInScene.Clear();
    }

    public void ChangeTextToNewLanguage()
    {
        foreach (var localizedText in localizedTextsInScene)
        {
            localizedText.Text = GetValue(localizedText.Key);
            //Debug.LogError("ChangeTextToNewLanguage: " + localizedText.Text + " " + localizedText.Key);
        }
    }

    public string GetValue(string key)
    {
        var result = string.Empty;
        return result = localizationTextDictionary.TryGetValue(key, out result) ? result : missingText;
    }

    public void ChangeLanguage(string newLanguage)
    {
        if (currentLanguage.Equals(newLanguage))
        {
            Debug.LogWarning("Language is already: " + newLanguage);
            return;
        }

        LoadLocalizedText(newLanguage);

        currentLanguage = newLanguage;

        ChangeTextToNewLanguage();
    }

    #endregion CUSTOM_FUNCTIONS
}
