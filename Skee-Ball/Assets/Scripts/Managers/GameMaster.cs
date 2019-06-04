using System.Collections;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class GameMaster : SingeltonPersistant<GameMaster>
{
    #region VARIABLES

    private Coroutine iStartGame_Coroutine;
    private BallEngine spawnedTrainingBall;
    private TrainingTarget trainingTarget;
    private Vector3 trainingTargetPosition = new Vector3(0, 1.5f, 1.4f);

    private readonly float trainingBallLifeSpan = 2f;

    #endregion VARIABLES

    #region PROPERTIES

    public Vector3 TrainingBallSpawnPoint
    {
        get
        {
            return Player.instance.feetPositionGuess + new Vector3(0, .5f, .5f);
        }
    }

    #endregion PROPERTIES

    #region UNITY_FUNCTIONS

    #endregion UNITY_FUNCTIONS

    #region CUSTOM_FUNCTIONS

    public GameObject SpawnGameObjectInstance(GameObject prefab, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion(), Transform parent = null, bool isActive = true)
    {
        var newInstance = Instantiate(prefab, position, rotation, parent);
        newInstance.SetActive(isActive);
        newInstance.name = prefab.name;

        return newInstance;
    }

    private void DespawnGameObjectInstance(GameObject instance)
    {
        if (instance != null)
        {
            AudioPlayer.Instance.PlayClipAtPoint(2, "DespawnSound", instance.transform.position);
            Destroy(instance);
        }
    }

    public void StartTrainingSession()
    {
        if (iStartGame_Coroutine == null)
        {
            iStartGame_Coroutine = StartCoroutine(IStartTrainingSession());
        }
    }

    private IEnumerator IStartTrainingSession()
    {
        AudioPlayer.Instance.PlayClipAtPoint(2, "SessionStarted", transform.position);

        // Show Menu / Options panel
        UIManager.Instance.ShowMenuPanel();

        yield return new WaitUntil(() => UIManager.Instance.IsOptionsConfirmed);

        if (spawnedTrainingBall == null)
        {
            spawnedTrainingBall = SpawnGameObjectInstance(ResourceManager.Instance.BallPrefab, TrainingBallSpawnPoint).GetComponent<BallEngine>();
            spawnedTrainingBall.BallLifetime = trainingBallLifeSpan;
            spawnedTrainingBall.PlaySpawnEffect();

            AudioPlayer.Instance.PlayClipAtPoint(2, "SpawnSound", TrainingBallSpawnPoint);
        }

        if (trainingTarget == null)
        {
            trainingTarget = SpawnGameObjectInstance(ResourceManager.Instance.TrainingTargetPrefab, trainingTargetPosition).GetComponent<TrainingTarget>();
        }

        // Show Tutorial / Hint panel -- Pick the ball hint
        UIManager.Instance.ShowTutorialPanel(1);

        yield return new WaitUntil(() => spawnedTrainingBall.IsPickedUp);

        UIManager.Instance.ShowTutorialPanel(2);
        // Show Tutorial / Hint panel -- Throw ball to target hint

        trainingTarget.gameObject.SetActive(true);

        yield return new WaitUntil(() => spawnedTrainingBall.IsPickedUp == false);

        // Close Panels and HUD
        UIManager.Instance.HideHUD();

        yield return new WaitUntil(() => trainingTarget.gameObject.activeSelf == false);

        SceneManager.Instance.ChangeNextScene();

        iStartGame_Coroutine = null;
    }

    private IEnumerator IStartNarrationSession()
    {


        yield return null;
    }

    private IEnumerator IStartGameSession()
    {


        yield return null;
    }

    #endregion CUSTOM_FUNCTIONS

}
