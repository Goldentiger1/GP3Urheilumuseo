using System;
using UnityEngine;
using UnityEngine.UI;

public class ControlPanel : MonoBehaviour
{
    private Button panelButton;
    private Button fiButton;
    private Button ukButton;

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        AssignButtonEvents();
    }

    private void Initialize()
    {
        var buttonContainer = transform.Find("ButtonContainer");

        panelButton = buttonContainer.Find("PanelButton").GetComponent<Button>();
        fiButton = buttonContainer.Find("FIButton").GetComponent<Button>();
        ukButton = buttonContainer.Find("UKButton").GetComponent<Button>();
    }

    private void AssignButtonEvents()
    {
        panelButton.onClick.AddListener(PanelButton);
        fiButton.onClick.AddListener(FIButton);
        ukButton.onClick.AddListener(UKButton);
    }

    private void PanelButton()
    {    
        if (GameMaster.Instance.CurrentSceneIndex == 0)
        {
            GameMaster.Instance.ChangeNextScene(); 
        }
        else
        {
            GameMaster.Instance.RestartScene();
        }
    }

    private void FIButton()
    {
        LocalizationManager.Instance.ChangeLanguage("FI");
    }

    private void UKButton()
    {
        LocalizationManager.Instance.ChangeLanguage("UK");
    }
}
