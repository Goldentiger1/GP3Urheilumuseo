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
        var buttonContainer = transform.Find("ButtonContainer");

        panelButton = buttonContainer.Find("PanelButton").GetComponent<Button>();
        fiButton = buttonContainer.Find("FIButton").GetComponent<Button>();
        ukButton = buttonContainer.Find("UKButton").GetComponent<Button>();
    }

    private void Start()
    {
        panelButton.onClick.AddListener(

            PanelButton

            );
        fiButton.onClick.AddListener(FINButton);
        ukButton.onClick.AddListener(UKButton);
    }

    private void PanelButton()
    {
        if(GameMaster.Instance.CurrentSceneIndex == 0)
        {
            OnStart();
        }
        else
        {
            OnRestart();
        }
    }

    private void OnStart()
    {
        GameMaster.Instance.ChangeScene(1);
    }

    private void OnRestart()
    {
        GameMaster.Instance.RestartScene();
    }

    private void FINButton()
    {
        LocalizationManager.Instance.ChangeLanguage("FI");
    }

    private void UKButton()
    {
        LocalizationManager.Instance.ChangeLanguage("UK");
    }
}
