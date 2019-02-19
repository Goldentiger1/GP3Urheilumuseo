using UnityEngine;
using UnityEditor;
using System.IO;

public class LocalizedEditor : EditorWindow
{
    #region VARIABLES

    public LocalizationData LocalizationData;

    #endregion VARIABLES

    #region UNITY_FUNCTIONS

    private void OnGUI()
    {
        if (LocalizationData != null)
        {
            var serializedObject = new SerializedObject(this);
            var serializedProperty = serializedObject.FindProperty("LocalizationData");
            EditorGUILayout.PropertyField(serializedProperty, true);
            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Save localization data"))
            {
                SaveLocalizationData();
            }
        }

        if (GUILayout.Button("Load localization data"))
        {
            LoadLocalizationData();
        }

        if (GUILayout.Button("Create new localization data"))
        {
            CreateNewLocalizationData();
        }
    }

    #endregion UNITY_FUNCTIONS

    #region CUSTOM_FUNCTIONS

    [MenuItem("Window/Localized Text Editor")]
    private static void Initialize()
    {
        GetWindow(typeof(LocalizedEditor)).Show();
    }

    private void LoadLocalizationData()
    {
        var filePath = EditorUtility.OpenFilePanel("Select localization data file",
            Application.streamingAssetsPath,
            "json");

        if (string.IsNullOrEmpty(filePath))
        {
            var dataAsJson = File.ReadAllText(filePath);
            LocalizationData = JsonUtility.FromJson<LocalizationData>(dataAsJson);
        }
    }

    private void SaveLocalizationData()
    {
        var filePath = EditorUtility.SaveFilePanel("Save localization data file",
            Application.streamingAssetsPath,
            "",
            "json");

        if (!string.IsNullOrEmpty(filePath))
        {
            string dataAsJson = JsonUtility.ToJson(LocalizationData);
            File.WriteAllText(filePath, dataAsJson);
        }
    }

    private void CreateNewLocalizationData()
    {
        LocalizationData = new LocalizationData();
    }

    #endregion CUSTOM_FUNCTIONS
}
