using UnityEngine;
using Valve.VR.InteractionSystem;

public class Standpoint : MonoBehaviour
{
    private GameObject SpawnedTrainingBall;

    public GameObject BallPrefab;

    public Vector3 BallSpawnPoint;

    private bool isFirstTimeTrigger;

    private GameObject SwitchScene_Icon;
    private GameObject Arrow_Icon;
    private GameObject Locked_Icon;
    private GameObject Feet_Icon;

    private new Renderer renderer;

    private AudioSource audioSource;

    private void Awake()
    {
        var icons = transform.GetChild(1);
        SwitchScene_Icon = icons.GetChild(0).gameObject;
        Arrow_Icon = icons.GetChild(1).gameObject;
        Locked_Icon = icons.GetChild(2).gameObject;
        Feet_Icon = icons.GetChild(3).gameObject;
        renderer = GetComponentInChildren<Renderer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
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
            SpawnedTrainingBall = Instantiate(BallPrefab, BallSpawnPoint, Quaternion.identity);

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

            renderer.enabled = false;
            Feet_Icon.SetActive(false);

            if (other.gameObject.layer.Equals(12)) 
            {
                Arrow_Icon.SetActive(false);
                audioSource.loop = false;

                SpawnTrainingBall();

                //UIManager.Instance
            }
        }     
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
