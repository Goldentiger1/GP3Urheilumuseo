using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singelton<UIManager>
{
    public Transform HUDCanvas { get; private set; }

    //private Transform panel;
    private Text scoreText, PowerText, LaunchPositionText;

    private void Awake()
    {
        HUDCanvas = GameObject.Find("HUDCanvas").transform;
        //panel = HUDCanvas.transform.Find("Panel");
        scoreText = HUDCanvas.transform.Find("ScoreText").GetComponent<Text>();
        PowerText = HUDCanvas.transform.Find("PowerText").GetComponent<Text>();
        LaunchPositionText = HUDCanvas.transform.Find("LaunchPositionText").GetComponent<Text>();
    }

    public void UpdateScoreText(int newScore)
    {
        scoreText.text = "SCORE: " + newScore.ToString("0");
    }

    public void UpdatePowerText(float newPower)
    {
        PowerText.text = "POWER: " + newPower.ToString("0.00");
    }

    public void UpdateLaunchPositionText(Vector3 newLaunchPosition)
    {
        LaunchPositionText.text = "LAUNCH POSITION: " + newLaunchPosition.ToString();
    }
}
