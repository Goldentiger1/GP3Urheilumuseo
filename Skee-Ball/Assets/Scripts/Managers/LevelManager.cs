using UnityEngine;

public class LevelManager : Singelton<LevelManager>
{
    public int Score { get; private set; }

    public Transform ScoreTrigger { get; private set; }

    private void Awake()
    {
        ScoreTrigger = GameObject.FindGameObjectWithTag("ScoreTrigger").transform;
    }

    private void Start()
    {
        Score = 0;
    }

    public void AddScore(int amount)
    {
        Score += amount;

        //Update score UI
        UIManager.Instance.UpdateScoreText(Score);

        //AudioManager.Instance.PlayClipAtPoint("Swish", ScoreTrigger.position);
    }
}
