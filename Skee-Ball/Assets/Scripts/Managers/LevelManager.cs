using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class LevelManager : Singelton<LevelManager>
{
    #region VARIABLES

    private Coroutine iStartGame_Coroutine;
    private Coroutine iStartTimer_Coroutine;

    [Range(0, 200)]
    public float LevelTime = 60f;

    private const int MAX_SCORE_AMOUNT = 10;

    private Stack<BallEngine> basketballs = new Stack<BallEngine>();

    public ScorePanel CurrentScorePanel { get; set; }

    private readonly float throwDistanceRequiredForThreePoints = 7f;

    private int totalScore = 0;

    #endregion VARIABLES

    #region PROPERTIES

    public bool IsGameStarted
    {
        get;
        set;
    }

    #endregion PROPERTIES

    #region UNITY_FUNCTIONS


    #endregion UNITY_FUNCTIONS

    #region CUSTOM_FUNCTIONS

    public void AddBasketball(BallEngine ballEngine)
    {
        basketballs.Push(ballEngine);
    }

    public void ClearBasketBalls()
    {
        basketballs.Clear();
    }

    public void UpdateScore(Transform hitTransform)
    {
        var throwDistance = Vector3.Distance(hitTransform.position, Player.instance.feetPositionGuess);

        totalScore += throwDistance > throwDistanceRequiredForThreePoints ? 3 : 2;
        CurrentScorePanel.UpdateScoreDisplayText(totalScore);
    }

    public void StartScene()
    {
        if (iStartGame_Coroutine == null)
            iStartGame_Coroutine = StartCoroutine(IStartScene());
    }

    private void StartLevelTimer()
    {
        if (iStartTimer_Coroutine == null)
        {
            iStartTimer_Coroutine = StartCoroutine(IStartLevelTimer());
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

        iStartTimer_Coroutine = null;

        yield return null;
    }

    public void ResetBallPostions()
    {
        foreach (var basketBall in basketballs)
        {
            basketBall.ResetPosition();
        }
    }

    private IEnumerator IStartScene()
    {
        var currentScene = SceneManager.Instance.CurrentScene;

        totalScore = 0;
        IsGameStarted = false;

        UIManager.Instance.FadeScreenOut();
        AudioPlayer.Instance.PlayMusicTrack(currentScene.Index);

        if (SceneManager.Instance.IsFirstScene == false)
        {
            if (currentScene.NarrationIndex != 0)
            {
                AudioPlayer.Instance.PlayNarration(currentScene.NarrationIndex);
            }

            if (CurrentScorePanel != null)
            {
                CurrentScorePanel.UpdateTimeDisplayText(LevelTime);
            }

            yield return new WaitUntil(() => IsGameStarted);

            StartLevelTimer();

            LocalizationManager.Instance.ChangeTextToNewLanguage();

            iStartGame_Coroutine = null;

            yield return null;
        }


    }

    #endregion CUSTOM_FUNCTIONS
}
