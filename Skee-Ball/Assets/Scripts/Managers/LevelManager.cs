using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class LevelManager : Singelton<LevelManager>
{
    private Coroutine iStartTimer;

    [Range(0, 200)]
    public float LevelTime = 60f;

    private const int MAX_SCORE_AMOUNT = 10;

    private Stack<BallEngine> basketBalls = new Stack<BallEngine>();

    public ScorePanel CurrentScorePanel { get; set; }

    private readonly float throwDistanceRequiredForThreePoints = 7f;

    private int totalScore = 0;

    private void Start()
    {
        
    }

    public void AddLevelBasketBall(BallEngine ballEngine)
    {
        basketBalls.Push(ballEngine);
    }

    public void ClearBasketBalls()
    {
        basketBalls.Clear();
    }

    private void Initialize()
    {

    }

    public void UpdateScore(Transform hitTransform)
    {
        var throwDistance = Vector3.Distance(hitTransform.position, Player.instance.feetPositionGuess);

        totalScore += throwDistance > throwDistanceRequiredForThreePoints ? 3 : 2;
        CurrentScorePanel.UpdateScoreDisplayText(totalScore);

       
        //SceneManager.Instance.ChangeNextScene();
             
    }

    private void StartLevelTimer()
    {
        if (SceneManager.Instance.IsFirstScene)
        {
            return;
        }

        if(iStartTimer == null)
        {
            iStartTimer = StartCoroutine(IStartLevelTimer());
        }
    }

    private IEnumerator IStartLevelTimer()
    {
        var startTime = LevelTime;

        while (startTime > 0)
        {
            startTime -= Time.deltaTime;

            //CurrentScorePanel.UpdateTimeDisplayText(startTime);
        }

        //yield return new WaitForSeconds(LevelTime);

        SceneManager.Instance.ChangeNextScene();

        iStartTimer = null;

        yield return null;
    }

    public void ResetBallPostions()
    {
        foreach (var basketBall in basketBalls)
        {
            basketBall.ResetPosition();
        }
    }

    public void ResetLevelValues()
    {
        totalScore = 0;
        StartLevelTimer();
    }
}
