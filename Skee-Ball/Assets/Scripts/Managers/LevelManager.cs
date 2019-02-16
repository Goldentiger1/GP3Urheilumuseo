using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class LevelManager : Singelton<LevelManager>
{
    private const int MAX_SCORE_AMOUNT = 10;

    private Stack<BallEngine> basketBalls = new Stack<BallEngine>();

    private readonly float throwDistanceRequiredForThreePoints = 7f;

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
        var throwDistance = Vector3.Distance(hitTransform.position, Player.instance.feetPositionGuess);

        totalScore = throwDistance > throwDistanceRequiredForThreePoints ? 3 : 2;

        if (totalScore < MAX_SCORE_AMOUNT)
        {

        }
        else
        {
            SceneManager.Instance.ChangeNextScene();
        }      
    }

    public void ResetScores()
    {
        totalScore = 0;
    }
}
