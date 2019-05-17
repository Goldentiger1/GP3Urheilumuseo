using System.Collections;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class BallEngine : Throwable
{
    #region VARIABLES

    private GameObject spawnEffect;

    private readonly float spinSpeed = 0.25f;
    private Coroutine iLifetimeCoroutine;
    private Coroutine iResetPositionCoroutine;

    private Vector3 rigidbodyStartPosition;
    private Quaternion rigidbodyStartRotation;

    private SphereCollider ballCollider;

    private readonly float minHitToSoundVelocity = 1f;
    private AudioSource audioSource;

    private TrailRenderer throwTrailEffect;
    private MeshRenderer ballRenderer;

    #endregion VARIABLES

    #region PROPERTIES

    public float CurrentVelocity
    {
        get
        {
            return rigidbody.velocity.magnitude;
        }
    }
    public float BallLifetime { get; set; } = 10f;
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

        ballCollider = GetComponent<SphereCollider>();

        audioSource = GetComponent<AudioSource>();
        throwTrailEffect = GetComponentInChildren<TrailRenderer>();
        ballRenderer = GetComponentInChildren<MeshRenderer>();

        Debug.LogError("FIX ME!!!!");
        // GameMaster should not be called by instances in the scene...
        // Now works because GameMaster executer order is before BallEngine ("this class")...
        spawnEffect = GameMaster.Instance.SpawnGameObjectInstance(ResourceManager.Instance.ObjectSpawnEffect, Vector3.zero, Quaternion.identity, transform);
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
                    0,
                      audioSource,
                      "Sock");

                break;

            case 14:
                // TrainingTarget

                AudioPlayer.Instance.PlaySfx(
                    2,
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
                        0,
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
                    0,
                      audioSource,
                      "Sock");

                break;

            case 14:
                // TrainingTarget

                AudioPlayer.Instance.PlaySfx(
                    2,
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
                        0,
                    audioSource,
                    "BackBoard");
                }

                break;

            case 17:
                //SpawnZone

                break;

            case 18:

                ResetPosition();

                if(iLifetimeCoroutine != null)
                {
                    StopCoroutine(iLifetimeCoroutine);
                }

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
        if(iResetPositionCoroutine == null)
        {
            iResetPositionCoroutine = StartCoroutine(IResetPosition());
        }
    }

    public void StartLifeTime(float lifeTime)
    {
        if (iLifetimeCoroutine == null)
        {
            iLifetimeCoroutine = StartCoroutine(IStartLifetime(BallLifetime));
        }      
    }

    protected override void OnAttachedToHand(Hand hand)
    {
        if(iLifetimeCoroutine != null)
        {
            StopCoroutine(iLifetimeCoroutine);
            iLifetimeCoroutine = null;
        }

        IsPickedUp = true;

        base.OnAttachedToHand(hand);
    }

    protected override void OnDetachedFromHand(Hand hand)
    {
        StartLifeTime(BallLifetime);

        base.OnDetachedFromHand(hand);

        var spinDirection = Vector3.Cross(rigidbody.velocity, Vector3.up).normalized;
     
        AddSpin(spinDirection, spinSpeed, ForceMode.Impulse);

        IsPickedUp = false;

        throwTrailEffect.enabled = true;
    }

    public void PlaySpawnEffect()
    {
        if(spawnEffect != null)
        {
            spawnEffect.SetActive(true);
        }
    }

    #endregion CUSTOM_FUNCTIONS

    #region COROUTINES

    private IEnumerator IStartLifetime(float lifeDuration) 
    {  
        yield return new WaitForSeconds(lifeDuration);

        PlaySpawnEffect();

        ResetPosition();

        iLifetimeCoroutine = null;
    }

    private void SetActive(bool isActive)
    {
        if (isActive)
        {
            rigidbody.isKinematic = false;
            ballCollider.enabled = true;
            ballRenderer.enabled = true;
        }
        else
        {
            rigidbody.isKinematic = true;
            ballCollider.enabled = false;
            ballRenderer.enabled = false;

            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;

            rigidbody.position = rigidbodyStartPosition;
            rigidbody.rotation = rigidbodyStartRotation;
        }
    }

    private IEnumerator IResetPosition()
    {
        SetActive(false);

        yield return new WaitWhile(() => spawnEffect.activeSelf);

        PlaySpawnEffect();

        yield return new WaitForEndOfFrame();

        SetActive(true);

        iResetPositionCoroutine = null;
    }

    #endregion COROUTINES
}
