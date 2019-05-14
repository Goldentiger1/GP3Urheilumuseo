using TMPro;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class TutorialPanel : UI_Panel
{
    private TextMeshProUGUI turtorialText;

    private void Awake()
    {
        turtorialText = GetComponentInChildren<TextMeshProUGUI>();
    }
}
