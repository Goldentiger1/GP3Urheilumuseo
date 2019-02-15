using UnityEngine;

public class Standpoint : MonoBehaviour
{
    public GameObject BallPrefab;

    public Vector3 BallSpawnPoint;

    public Light RoomPointLight;

    private GameObject SwitchScene_Icon;
    private GameObject Arrow_Icon;
    private GameObject Locked_Icon;
    private GameObject Feet_Icon;

    private AudioSource audioSource;

    private bool playerInArea;

    private void Awake()
    {
        var icons = transform.GetChild(1);
        SwitchScene_Icon = icons.GetChild(0).gameObject;
        Arrow_Icon = icons.GetChild(1).gameObject;
        Locked_Icon = icons.GetChild(2).gameObject;
        Feet_Icon = icons.GetChild(3).gameObject;

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
        Instantiate(BallPrefab, BallSpawnPoint, Quaternion.identity);

        AudioPlayer.Instance.PlayClipAtPoint("SpawnSound", BallSpawnPoint);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(12))
        {
            playerInArea = true;
            Arrow_Icon.SetActive(false);
            audioSource.loop = false;

            Debug.LogError("OnTriggerEnter: " + other.name);

            //GameMaster.Instance.ChangeNextScene();
            SpawnTrainingBall();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer.Equals(12))
        {
            playerInArea = false;
            Arrow_Icon.SetActive(true);

            if(audioSource.isPlaying == true)
            AudioPlayer.Instance.PlayLoopingSfx(audioSource, "StandpointLoop");

            Debug.LogError("OnTriggerExit: " + other.name);
        }
    }
}
