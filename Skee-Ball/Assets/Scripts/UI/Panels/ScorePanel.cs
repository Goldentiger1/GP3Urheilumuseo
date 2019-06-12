using TMPro;

public class ScorePanel : UI_Panel
{
    private TextMeshProUGUI timeDisplayText;
    private TextMeshProUGUI scoreDisplayText;

    private void Awake()
    {
        timeDisplayText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        scoreDisplayText = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        LevelManager.Instance.CurrentScorePanel = this;
    }

    public void UpdateScoreDisplayText(int newValue)
    {
        scoreDisplayText.text = " " + newValue.ToString();
    }

    public void UpdateTimeDisplayText(float newValue)
    {
        timeDisplayText.text = ": " + newValue.ToString("00.0");
    }
}
