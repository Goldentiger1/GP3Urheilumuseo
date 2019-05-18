using TMPro;

public class TutorialPanel : UI_Panel
{
    private TextMeshProUGUI turtorialText_1;
    private TextMeshProUGUI turtorialText_2;

    private void Awake()
    {
        var tutorialTexts = GetComponentsInChildren<TextMeshProUGUI>();
        turtorialText_1 = tutorialTexts[0];
        turtorialText_2 = tutorialTexts[1];
    }

    public void ShowTutorialText(int tutorialTextNumber)
    {
        if (tutorialTextNumber == 1)
        {
            turtorialText_2.gameObject.SetActive(false);
            turtorialText_1.gameObject.SetActive(true);

        } 
        else
        {       
            turtorialText_1.gameObject.SetActive(false);
            turtorialText_2.gameObject.SetActive(true);
        }
    }
}
