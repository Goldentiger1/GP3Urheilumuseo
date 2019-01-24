using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreCounter : MonoBehaviour
{
    public TextMeshProUGUI uiScoreText;
    public TextMeshProUGUI uiScoreNumber;
    public float SceneChangeTimer = 60f;
    public Vector3 throwStart;
    public float throwDistance;
    public float throwDistanceRequiredForThreePoints = 7f;
    private readonly float sceneChangeWaitTime = 2f;

    int score = 0;

    private void Update()
    {
        SceneChangeTimer -= Time.deltaTime;
        if(SceneChangeTimer <= 0)
        {
            print("!!!");
          //  ChangeScene();
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

        if (throwDistance > throwDistanceRequiredForThreePoints)
        {
            AddScore(3);
        }
        else
        {
            AddScore(2);
        }

        if (score < 10)
        {
            uiScoreNumber.text = "0" + score;
        }
        else
        {
            uiScoreNumber.text = "" + score;
            Invoke( "ChangeScene", sceneChangeWaitTime);
        }
    }
    
    private void ChangeScene()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;

        switch (currentScene)
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
