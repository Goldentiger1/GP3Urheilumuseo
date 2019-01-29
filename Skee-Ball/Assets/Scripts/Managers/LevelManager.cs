using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class LevelManager : Singelton<LevelManager>
{
    private const int MAX_SCORE_AMOUNT = 10;

    private Stack<BallEngine> basketBalls = new Stack<BallEngine>();

    private float SceneChangeTimer = 2f;
    private float throwDistance;
    private readonly float throwDistanceRequiredForThreePoints = 7f;
    private readonly float sceneChangeWaitTime = 2f;

    private int totalScore = 0;

    private void Awake()
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
        throwDistance = Vector3.Distance(hitTransform.position, Player.Instance.FeetPositionGuess);

        totalScore = throwDistance > throwDistanceRequiredForThreePoints ? 3 : 2;

        if (totalScore < MAX_SCORE_AMOUNT)
        {
            UIManager.Instance.UpdateScoreVisuals(totalScore.ToString());
        }
        else
        {
            UIManager.Instance.UpdateScoreVisuals(totalScore.ToString());
        }      
    }
}
