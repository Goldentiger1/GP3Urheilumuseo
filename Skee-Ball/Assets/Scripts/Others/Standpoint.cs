using System.Collections;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Standpoint : MonoBehaviour
{
    private BallEngine spawnedTrainingBall;

    public GameObject BallPrefab;
    public GameObject TrainingTarget;

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

        TrainingTarget.SetActive(false);
    }

    private void OnStart()
    {
        SwitchScene_Icon.SetActive(false);
        Arrow_Icon.SetActive(true);
        Locked_Icon.SetActive(false);
        Feet_Icon.SetActive(true);

        if (audioSource.isPlaying == false)
            AudioPlayer.Instance.PlayLoopingSfx(audioSource, "StandpointLoop");
    }

    private void SpawnTrainingBall() 
    {
        if(spawnedTrainingBall == null) 
        {
            ballSpawnPoint = Player.instance.feetPositionGuess + new Vector3(0, 0.2f, 0.5f);
            spawnedTrainingBall = Instantiate(BallPrefab, ballSpawnPoint, Quaternion.identity).GetComponent<BallEngine>();
            spawnedTrainingBall.name = BallPrefab.name;
            AudioPlayer.Instance.PlayClipAtPoint("SpawnSound", ballSpawnPoint);
        }     
    }

    private void UnspawnTrainingBall() 
    {
        if(spawnedTrainingBall != null) 
        {
            AudioPlayer.Instance.PlayClipAtPoint("DespawnSound", ballSpawnPoint);
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

                UIManager.Instance.ShowHUD(Player.instance.bodyDirectionGuess + Vector3.forward, 1);
            }
        }     
    }

    private void StartGame()
    {
        StartCoroutine(IStartGame());
    }

    private IEnumerator IStartGame()
    {
        TrainingTarget.SetActive(true);

        yield return new WaitUntil(() => TrainingTarget.activeSelf == false);

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
