using System.Collections;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class GameMaster : SingeltonPersistant<GameMaster>
{
    #region VARIABLES

    private Coroutine iStartGame_Coroutine;
    private BallEngine spawnedTrainingBall;
    private TrainingTarget trainingTarget;

    private readonly float trainingBallLifeSpan = 2f;

    #endregion VARIABLES

    #region PROPERTIES

    public Vector3 TrainingBallSpawnPoint
    {
        get
        {
            return Player.instance.feetPositionGuess + new Vector3(0, 1, 1);
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
            iStartGame_Coroutine = StartCoroutine(IStartGame());
        }
    }

    private IEnumerator IStartGame()
    {
        UIManager.Instance.ShowHUD(
                    0,
                    Player.instance.bodyDirectionGuess + Vector3.forward,
                    1f,
                    400f);

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
            trainingTarget = Instantiate(ResourceManager.Instance.TrainingTargetPrefab).GetComponent<TrainingTarget>();
        }

        yield return new WaitUntil(() => spawnedTrainingBall.IsPickedUp);

        UIManager.Instance.HideHUD();

        UIManager.Instance.ShowHUD(
                   1,
                   Player.instance.bodyDirectionGuess + Vector3.forward,
                   1f,
                   400f);

        trainingTarget.gameObject.SetActive(true);
        // Localization...
        // UIManager.Instance.ChangeHintText("HEILAUTA OHJAINTA JA PÄÄSTÄ LIIPAISIMESTA HEITTÄÄKSESI PALLOA. YRITÄ OSUA EDESSÄSI OLEVAAN MAALITAULUUN.");

        yield return new WaitUntil(() => trainingTarget.gameObject.activeSelf == false);

        UIManager.Instance.HideHUD();

        SceneManager.Instance.ChangeNextScene();
    }

    #endregion CUSTOM_FUNCTIONS

}
