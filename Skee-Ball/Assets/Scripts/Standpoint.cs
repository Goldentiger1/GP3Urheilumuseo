using System.Collections;
using UnityEngine;

public class Standpoint : MonoBehaviour
{
    private BallEngine SpawnedTrainingBall;

    public GameObject BallPrefab;

    public Vector3 BallSpawnPoint;

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
            AudioPlayer.Instance.PlayLoopingSfx(audioSource, "StandpointLoop");
    }

    private void SpawnTrainingBall() 
    {
        if(SpawnedTrainingBall == null) 
        {       
            SpawnedTrainingBall = Instantiate(BallPrefab, BallSpawnPoint, Quaternion.identity).GetComponent<BallEngine>();

            AudioPlayer.Instance.PlayClipAtPoint("SpawnSound", BallSpawnPoint);
        }     
    }

    private void UnspawnTrainingBall() 
    {
        if(SpawnedTrainingBall != null) 
        {
            AudioPlayer.Instance.PlayClipAtPoint("DespawnSound", BallSpawnPoint);
            Destroy(SpawnedTrainingBall);
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

                // UIManager.Instance
            }
        }     
    }

    private void StartGame()
    {
        StartCoroutine(IStartGame());
    }

    private IEnumerator IStartGame()
    {
        yield return new WaitUntil(() => SpawnedTrainingBall.IsThrowed);

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
