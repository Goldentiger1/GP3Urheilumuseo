using System.Collections.Generic;
using System.IO;
using UnityEngine;

[SerializeField]
public class LocalizationData
{
    public LocalizationItem[] Items;
}

[SerializeField]
public class LocalizationItem
{
    public string Key;
    public string Value;
}

public class LocalizationManager : Singelton<LocalizationManager>
{
    private Dictionary<string, string> localizationText;

    private void Start()
    {
        //LoadLocalizedText("LocalizedText_UK.json");
    }

    public void LoadLocalizedText(string fileName)
    {
        localizationText = new Dictionary<string, string>();

        string filePath = Path.Combine(Application.streamingAssetsPath, fileName );
        Debug.Log(filePath);

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);

            print("Json: " + dataAsJson);

            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);
            print("Loaded Items: " + loadedData.Items.Length);

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
    }
}
