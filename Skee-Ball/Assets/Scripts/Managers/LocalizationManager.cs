﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public enum LANGUAGE
{
    FI,
    UK
}

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

    private Dictionary<string, string> localizationTextDictionary;
    private List<LocalizedText> localizedTextsInScene = new List<LocalizedText>();

    public LANGUAGE DEFAULT_LANGUAGE = LANGUAGE.FI;

    public LANGUAGE CURRENT_LANGUAGE
    {
        get;
        private set;
    }

    public LANGUAGE PREVIOUS_LANGUAGE
    {
        get;
        private set;
    }

    public string MissingText
    {
        get 
        {
            switch (CURRENT_LANGUAGE)
            {
                case LANGUAGE.FI:

                return "Lokalisoitua tekstiä ei löytynyt!";

                case LANGUAGE.UK:

                return "Localized text not found!";

                default:

                return "Lokalisoitua tekstiä ei löytynyt!";
            }

        }
    } 

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

        SetLanguage(DEFAULT_LANGUAGE);
    }

    #endregion UNITY_FUNCTIONS

    #region CUSTOM_FUNCTIONS

    private void LoadLocalizedText(LANGUAGE fileName)
    {
        localizationTextDictionary = new Dictionary<string, string>();

        var filePath = Path.Combine(Application.streamingAssetsPath, "LocalizedText_" + fileName.ToString() + ".json");

        if (File.Exists(filePath))
        {
            var dataAsJson = File.ReadAllText(filePath, Encoding.Default);

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
        return localizationTextDictionary.TryGetValue(key, out result) ? result : MissingText;
    }

    private void SetLanguage(LANGUAGE NEW_LANGUAGE)
    {
        LoadLocalizedText(NEW_LANGUAGE);

        ChangeTextToNewLanguage();

        CURRENT_LANGUAGE = NEW_LANGUAGE;
    }

    public void ChangeLanguage(LANGUAGE NEW_LANGUAGE)
    {
        if (CURRENT_LANGUAGE.Equals(NEW_LANGUAGE))
        {
            Debug.LogWarning("Language is already: " + NEW_LANGUAGE);
            return;
        }

        SetLanguage(NEW_LANGUAGE);
    }

    #endregion CUSTOM_FUNCTIONS
}
