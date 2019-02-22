using UnityEngine;
using Valve.VR.InteractionSystem;

public class BallEngine : Throwable
{
    #region VARIABLES

    private readonly float spinSpeed = 0.25f;

    private Vector3 rigidbodyStartPosition;
    private readonly float minHitToSoundVelocity = 1f;
    private AudioSource audioSource;

    private bool colliderUpIsTrigged, colliderDownIsTrigged;

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

    private void Start()
    {
        LevelManager.Instance.AddLevelBasketBall(this);

        rigidbodyStartPosition = rigidbody.transform.position;
        throwTrailEffect.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(13))
        {
            LevelManager.Instance.UpdateScore(other.transform);

            AudioPlayer.Instance.PlaySfx(
                  audioSource,
                  "Sock");
        }

        if (other.gameObject.layer.Equals(14))
        {

            AudioPlayer.Instance.PlaySfx(
                   audioSource,
                   "IncreaseScore");

            other.gameObject.SetActive(false);
        }

        if (other.gameObject.layer.Equals(15))
        {

            //AudioPlayer.Instance.PlaySfx(
            //       audioSource,
            //       "IncreaseScore");

            Destroy(gameObject, 2f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer.Equals(13))
        {
            
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        throwTrailEffect.enabled = false;

        switch (collision.gameObject.layer)
        {
            case 9:

                if (rigidbody.velocity.magnitude < minHitToSoundVelocity)
                {
                    return;
                }

                AudioPlayer.Instance.PlaySfx(
                    audioSource,
                    "BallBounce");

                break;

            case 10:

                if (rigidbody.velocity.magnitude < minHitToSoundVelocity)
                {
                    AudioPlayer.Instance.PlaySfx(
                    audioSource,
                    "BackBoard");

                    return;
                }

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
        rigidbody.position = rigidbodyStartPosition;
        rigidbody.rotation = Quaternion.Euler(Vector3.zero);
    }

    protected override void OnAttachedToHand(Hand hand)
    {
        IsPickedUp = true;
        base.OnAttachedToHand(hand);
    }

    protected override void OnDetachedFromHand(Hand hand)
    {
        base.OnDetachedFromHand(hand);

        var spinDirection = Vector3.Cross(rigidbody.velocity, Vector3.up).normalized;
     
        AddSpin(spinDirection, spinSpeed, ForceMode.Impulse);

        IsPickedUp = false;

        throwTrailEffect.enabled = true;
    }

    #endregion CUSTOM_FUNCTIONS
}
