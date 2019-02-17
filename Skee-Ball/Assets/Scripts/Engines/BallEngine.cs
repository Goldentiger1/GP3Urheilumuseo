using UnityEngine;
using Valve.VR.InteractionSystem;

public class BallEngine : Throwable
{
    private Vector3 rigidbodyStartPosition;
    private readonly float minHitToSoundVelocity = 1f;

    private AudioSource audioSource;

    public float CurrentVelocity
    {
        get
        {
            return rigidbody.velocity.magnitude;
        }
    }
    public bool IsAttached
    {
        get
        {
            return attached;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        LevelManager.Instance.AddLevelBasketBall(this);

        rigidbodyStartPosition = rigidbody.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(13))
        {
            LevelManager.Instance.UpdateScore(other.transform);

            AudioPlayer.Instance.PlaySfx(
                   audioSource,
                   "IncreaseScore");
        }
    }

    private void OnTriggerExit(Collider other)
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
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
                    "Sock");

                    return;
                }

                break;
        }
    }

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
        base.OnAttachedToHand(hand);
    }

    private void Update()
    {
        //print("Attach rotation: " + attachRotation);
    }

    protected override void OnDetachedFromHand(Hand hand)
    {
        base.OnDetachedFromHand(hand);

        //AddSpin(pla, 100, ForceMode.Impulse);
    }
}
