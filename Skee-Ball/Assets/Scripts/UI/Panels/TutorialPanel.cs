using TMPro;

public class TutorialPanel : UI_Panel
{
    private TextMeshProUGUI turtorialText_1;
    private TextMeshProUGUI turtorialText_2;

    private bool hasInitialized = false;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (hasInitialized)
        {
            return;
        }

        var tutorialTexts = GetComponentsInChildren<TextMeshProUGUI>();
        turtorialText_1 = tutorialTexts[0];
        turtorialText_2 = tutorialTexts[1];

        hasInitialized = true;
    }

    private void OnEnable()
    {
        Initialize();
    }

    public void ShowTutorialText(int tutorialTextNumber)
    {
        switch (tutorialTextNumber)
        {
            case 1:
            turtorialText_2.gameObject.SetActive(false);
            turtorialText_1.gameObject.SetActive(true);
            break;

            case 2:
            turtorialText_1.gameObject.SetActive(false);
            turtorialText_2.gameObject.SetActive(true);
            break;

            default:

            break;
        }
    }
}
