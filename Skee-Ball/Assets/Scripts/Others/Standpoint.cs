using System.Collections;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Standpoint : MonoBehaviour
{
    public Transform ballSpawnPosition;

    private BallEngine spawnedTrainingBall;
    private GameObject trainingTarget;

    private Vector3 ballSpawnPoint;

    private bool isFirstTimeTrigger;

    private Collider triggerColider;

    private GameObject SwitchScene_Icon;
    private GameObject Arrow_Icon;
    private GameObject Locked_Icon;
    private GameObject Feet_Icon;

    private new Renderer renderer;

    private AudioSource audioSource;

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        OnStart();
    }

    private void Initialize()
    {
        var icons = transform.GetChild(1);
        SwitchScene_Icon = icons.GetChild(0).gameObject;
        Arrow_Icon = icons.GetChild(1).gameObject;
        Locked_Icon = icons.GetChild(2).gameObject;
        Feet_Icon = icons.GetChild(3).gameObject;
        triggerColider = GetComponent<Collider>();
        renderer = GetComponentInChildren<Renderer>();
        audioSource = GetComponent<AudioSource>();      
    }

    private void OnStart()
    {
        SwitchScene_Icon.SetActive(false);
        Arrow_Icon.SetActive(true);
        Locked_Icon.SetActive(false);
        Feet_Icon.SetActive(true);

        if (audioSource.isPlaying == false)
            AudioPlayer.Instance.PlayLoopingSfx(2, audioSource, "StandpointLoop");

        var trainingTargetPrefab = ResourceManager.Instance.TrainingTargetPrefab;
        trainingTarget = Instantiate(trainingTargetPrefab);
        trainingTarget.name = trainingTargetPrefab.name;
    }

    private void SpawnTrainingBall() 
    {
        if(spawnedTrainingBall == null) 
        {
            ballSpawnPoint = ballSpawnPosition.position;
            var ballPrefab = ResourceManager.Instance.BallPrefab;
            spawnedTrainingBall = Instantiate(ballPrefab, ballSpawnPoint, Quaternion.identity).GetComponent<BallEngine>();
            spawnedTrainingBall.BallLifetime = 2f;
            spawnedTrainingBall.name = ballPrefab.name;
            AudioPlayer.Instance.PlayClipAtPoint(2, "SpawnSound", ballSpawnPoint);
        }     
    }

    private void UnspawnTrainingBall() 
    {
        if(spawnedTrainingBall != null) 
        {
            AudioPlayer.Instance.PlayClipAtPoint(2, "DespawnSound", ballSpawnPoint);
            Destroy(spawnedTrainingBall);
        }     
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isFirstTimeTrigger == false) 
        {
            isFirstTimeTrigger = true;

            triggerColider.enabled = false;
            renderer.enabled = false;
            Feet_Icon.SetActive(false);

            if (other.gameObject.layer.Equals(12)) 
            {
                Arrow_Icon.SetActive(false);
                audioSource.loop = false;

                SpawnTrainingBall();

                StartGame();

                UIManager.Instance.ShowHUD(Player.instance.bodyDirectionGuess + Vector3.forward, 1f, 400f);
            }
        }     
    }

    private void StartGame()
    {
        StartCoroutine(IStartGame());
    }

    private IEnumerator IStartGame()
    {
        trainingTarget.SetActive(true);

        yield return new WaitUntil(() => spawnedTrainingBall.IsPickedUp);

        // Localization...
        UIManager.Instance.ChangeHintText("HEILAUTA OHJAINTA JA PÄÄSTÄ LIIPAISIMESTA HEITTÄÄKSESI PALLOA. YRITÄ OSUA EDESSÄSI OLEVAAN MAALITAULUUN.");

        yield return new WaitUntil(() => trainingTarget.activeSelf == false);

        UIManager.Instance.HideHUD();

        SceneManager.Instance.ChangeNextScene();
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.layer.Equals(12))
    //    {
    //        Arrow_Icon.SetActive(true);

    //        if(audioSource.isPlaying == true)
    //        AudioPlayer.Instance.PlayLoopingSfx(audioSource, "StandpointLoop");

    //        UnspawnTrainingBall();
    //    }
    //}
}
