using UnityEngine;
using TMPro;

public class ScorePanel : MonoBehaviour
{
    private TextMeshProUGUI timeDisplayText;
    private TextMeshProUGUI scoreDisplayText;

    private void Awake()
    {
        timeDisplayText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        scoreDisplayText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        LevelManager.Instance.CurrentScorePanel = this;
    }

    public void UpdateScoreDisplayText(int newValue)
    {
        scoreDisplayText.text = newValue.ToString();
    }

    public void UpdateTimeDisplayText(float newValue)
    {
        scoreDisplayText.text = newValue.ToString();
    }
}
