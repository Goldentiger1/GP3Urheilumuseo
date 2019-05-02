using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class LevelManager : Singelton<LevelManager>
{
    #region VARIABLES

    private Coroutine iStartTimer;

    [Range(0, 200)]
    public float LevelTime = 60f;

    private const int MAX_SCORE_AMOUNT = 10;

    private Stack<BallEngine> basketBalls = new Stack<BallEngine>();

    public ScorePanel CurrentScorePanel { get; set; }

    private readonly float throwDistanceRequiredForThreePoints = 7f;

    private int totalScore = 0;

    #endregion VARIABLES

    #region PROPERTIES



    #endregion PROPERTIES

    #region UNITY_FUNCTIONS



    #endregion UNITY_FUNCTIONS

    #region CUSTOM_FUNCTIONS

    public void AddLevelBasketBall(BallEngine ballEngine)
    {
        basketBalls.Push(ballEngine);
    }

    public void ClearBasketBalls()
    {
        basketBalls.Clear();
    }

    public void UpdateScore(Transform hitTransform)
    {
        var throwDistance = Vector3.Distance(hitTransform.position, Player.instance.feetPositionGuess);

        totalScore += throwDistance > throwDistanceRequiredForThreePoints ? 3 : 2;
        CurrentScorePanel.UpdateScoreDisplayText(totalScore);
    }

    private void StartLevelTimer()
    {
        if (SceneManager.Instance.IsFirstScene)
        {
            return;
        }

        if (iStartTimer == null)
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

            if (CurrentScorePanel != null)
            {
                CurrentScorePanel.UpdateTimeDisplayText(startTime);
            }

            yield return null;
        }

        CurrentScorePanel.UpdateTimeDisplayText(0);

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

    #endregion CUSTOM_FUNCTIONS
}
