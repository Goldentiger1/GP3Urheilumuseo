using UnityEngine;
using TMPro;

public class ScorePanel : MonoBehaviour
{
    private TextMeshProUGUI timerDisplayText;
    private TextMeshProUGUI scoreDisplayText;

    private void Awake()
    {
        timerDisplayText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
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
}
