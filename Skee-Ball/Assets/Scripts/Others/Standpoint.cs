using System.Collections;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Standpoint : MonoBehaviour
{
    private Coroutine iStartGame_Coroutine;

    private BallEngine spawnedTrainingBall;
    private readonly float trainingBallLifeSpan = 2f;
    private GameObject trainingTarget;

    private bool isFirstTimeTrigger;

    private Collider triggerColider;

    private GameObject SwitchScene_Icon;
    private GameObject Arrow_Icon;
    private GameObject Locked_Icon;
    private GameObject Feet_Icon;

    private new Renderer renderer;

    private AudioSource audioSource;

    public Vector3 BallSpawnPoint 
    {
        get
        {
            return Player.instance.feetPositionGuess + Vector3.forward * 0.5f;
        }
    }

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
    }

    private void SpawnTrainingBall() 
    {
        if(spawnedTrainingBall == null) 
        {
            
            var ballPrefab = ResourceManager.Instance.BallPrefab;
            spawnedTrainingBall = Instantiate(ballPrefab, BallSpawnPoint, Quaternion.identity).GetComponent<BallEngine>();
            spawnedTrainingBall.BallLifetime = trainingBallLifeSpan;
            spawnedTrainingBall.name = ballPrefab.name;
            AudioPlayer.Instance.PlayClipAtPoint(2, "SpawnSound", BallSpawnPoint);
        }     
    }

    private void UnspawnTrainingBall() 
    {
        if(spawnedTrainingBall != null) 
        {
            AudioPlayer.Instance.PlayClipAtPoint(2, "DespawnSound", BallSpawnPoint);
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

               
            }
        }     
    }

    private void StartGame()
    {
        if(iStartGame_Coroutine == null)
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

        var trainingTargetPrefab = ResourceManager.Instance.TrainingTargetPrefab;
        trainingTarget = Instantiate(trainingTargetPrefab);
        trainingTarget.name = trainingTargetPrefab.name;

        yield return new WaitUntil(() => spawnedTrainingBall.IsPickedUp);    

        UIManager.Instance.ShowHUD(
                   1,
                   Player.instance.bodyDirectionGuess + Vector3.forward,
                   1f,
                   400f);


        trainingTarget.SetActive(true);
        // Localization...
        // UIManager.Instance.ChangeHintText("HEILAUTA OHJAINTA JA PÄÄSTÄ LIIPAISIMESTA HEITTÄÄKSESI PALLOA. YRITÄ OSUA EDESSÄSI OLEVAAN MAALITAULUUN.");

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
