using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    private const int MAX_SCORE_AMOUNT = 10;

    private TextMeshProUGUI uiScoreText;
    private Vector3 throwStart = Vector3.zero;
    private float SceneChangeTimer = 200f;
    private float throwDistance;
    private readonly float throwDistanceRequiredForThreePoints = 7f;
    private readonly float sceneChangeWaitTime = 2f;

    int score = 0;

    private void Awake()
    {
        uiScoreText = transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (GameMaster.Instance.IsChangingScene)
        {
            return;
        }

        SceneChangeTimer -= Time.deltaTime;

        if(SceneChangeTimer <= 0)
        {
            ChangeScene();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        UpdateScore();
    }

    private void AddScore(int scoreAmount)
    {
        Fabric.EventManager.Instance.PostEvent("score");
        score += scoreAmount;
    }

    private void UpdateScore()
    {    
        throwDistance = Vector3.Distance(throwStart, transform.position);

        AddScore(throwDistance > throwDistanceRequiredForThreePoints ? 3 : 2);

        if (score < MAX_SCORE_AMOUNT)
        {
            uiScoreText.text = "SCORE 0" + score;
        }
        else
        {
            uiScoreText.text = "SCORE " + score;
            Invoke( "ChangeScene", sceneChangeWaitTime);
        }
    }
    
    private void ChangeScene()
    {
        switch (GameMaster.Instance.CurrentSceneIndex)
        {
            case 1:
                
                GameMaster.Instance.ChangeScene(2);

                break;

            case 2:

                GameMaster.Instance.ChangeScene(0);

                break;

            default:

                break;
        }
    }
}
