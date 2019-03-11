using System.Collections;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class BallEngine : Throwable
{
    #region VARIABLES

    private readonly float spinSpeed = 0.25f;
    private readonly float ballLifetime = 20f;

    private Coroutine iLifetime;

    private Vector3 rigidbodyStartPosition;
    private Quaternion rigidbodyStartRotation;

    private readonly float minHitToSoundVelocity = 1f;
    private AudioSource audioSource;

    private TrailRenderer throwTrailEffect;

    #endregion VARIABLES

    #region PROPERTIES

    public float CurrentVelocity
    {
        get
        {
            return rigidbody.velocity.magnitude;
        }
    }
    public bool IsPickedUp
    {
        get;
        private set;
    }

    #endregion PROPERTIES

    #region UNITY_FUNCTIONS

    protected override void Awake()
    {
        base.Awake();

        audioSource = GetComponent<AudioSource>();
        throwTrailEffect = GetComponentInChildren<TrailRenderer>();
    }

    private void OnEnable()
    {
        IsPickedUp = false;
    }

    private void Start()
    {
        LevelManager.Instance.AddLevelBasketBall(this);

        rigidbodyStartPosition = rigidbody.transform.position;
        rigidbodyStartRotation = rigidbody.transform.rotation;
        throwTrailEffect.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        var hittedObject = other.gameObject;

        switch (hittedObject.layer)
        {
            case 9:
                // Floor

                break;

            case 10:
                // Hoop

                break;

            case 11:
                //Ball

                break;

            case 12:
                //Player

                break;

            case 13:
                //ScoreTrigger

                LevelManager.Instance.UpdateScore(hittedObject.transform);

                AudioPlayer.Instance.PlaySfx(
                      audioSource,
                      "Sock");

                break;

            case 14:
                // TrainingTarget

                AudioPlayer.Instance.PlaySfx(
                  audioSource,
                  "IncreaseScore");

                hittedObject.SetActive(false);

                break;

            case 15:
                // BallDestroyer

                //AudioPlayer.Instance.PlaySfx(
                //       audioSource,
                //       "IncreaseScore");

                Destroy(gameObject, 2f);

                break;

            case 16:
                //BackBoard


                //AudioPlayer.Instance.PlaySfx(
                //       audioSource,
                //       "IncreaseScore");

                Destroy(gameObject, 2f);

                break;

            case 17:
                //SpawnZone

                break;

            default:

                break;
        }      
    }

    private void OnTriggerExit(Collider other)
    {
      
    }

    private void OnCollisionEnter(Collision collision)
    {
        var hittedObject = collision.gameObject;

        throwTrailEffect.enabled = false;
        throwTrailEffect.Clear();

        switch (hittedObject.layer)
        {
            case 9:
                // Floor

                if (rigidbody.velocity.magnitude >= minHitToSoundVelocity)
                {
                    AudioPlayer.Instance.PlaySfx(
                        audioSource,
                        "BallBounce");
                }

                break;

            case 10:
                // Hoop

                

                break;

            case 11:
                //Ball

                break;

            case 12:
                //Player

                break;

            case 13:
                //ScoreTrigger

                LevelManager.Instance.UpdateScore(hittedObject.transform);

                AudioPlayer.Instance.PlaySfx(
                      audioSource,
                      "Sock");

                break;

            case 14:
                // TrainingTarget

                AudioPlayer.Instance.PlaySfx(
                  audioSource,
                  "IncreaseScore");

                hittedObject.SetActive(false);

                break;

            case 15:
                // BallDestroyer

                //AudioPlayer.Instance.PlaySfx(
                //       audioSource,
                //       "IncreaseScore");

                Destroy(gameObject, 2f);

                break;

            case 16:
                //BackBoard

                if (rigidbody.velocity.magnitude >= minHitToSoundVelocity)
                {
                    AudioPlayer.Instance.PlaySfx(
                    audioSource,
                    "BackBoard");
                }

                break;

            case 17:
                //SpawnZone

                break;

            default:

                break;
        }      
    }

    #endregion UNITY_FUNCTIONS

    #region CUSTOM_FUNCTIONS

    private void AddSpin(Vector3 spinDirection, float force, ForceMode forceMode)
    {
        rigidbody.AddTorque(spinDirection * force, forceMode);
    }

    public void ResetPosition()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        rigidbody.position = rigidbodyStartPosition;
        rigidbody.rotation = rigidbodyStartRotation;
    }

    protected override void OnAttachedToHand(Hand hand)
    {
        IsPickedUp = true;

        base.OnAttachedToHand(hand);
    }

    protected override void OnDetachedFromHand(Hand hand)
    {
        base.OnDetachedFromHand(hand);

        // !!!
        iLifetime = StartCoroutine(IStartLifetime(ballLifetime));

        var spinDirection = Vector3.Cross(rigidbody.velocity, Vector3.up).normalized;
     
        AddSpin(spinDirection, spinSpeed, ForceMode.Impulse);

        IsPickedUp = false;

        throwTrailEffect.enabled = true;
    }

    #endregion CUSTOM_FUNCTIONS

    #region COROUTINES

    private IEnumerator IStartLifetime(float lifeDuration) 
    {
        yield return new WaitForSeconds(lifeDuration);

        ResetPosition();
    }

    #endregion COROUTINES
}
