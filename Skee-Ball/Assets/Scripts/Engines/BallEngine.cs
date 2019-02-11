using UnityEngine;

public class BallEngine : MonoBehaviour
{
    private new Rigidbody rigidbody;
    private Vector3 rigidbodyStartPosition;
    private readonly float minHitToSoundVelocity = 1F;
    private readonly float spinSpeed = 10f;

    private AudioSource audioSource;

    #region AARO

    // Size of torque along the world x-axis
    public float torqueX;
    // Size of torque along the world y-axis
    public float torqueY;
    // Size of torque along the world z-axis
    public float torqueZ;
    // Rigidbody rotation speed
    public float speed;
    // Old rigidbody velocity
    public Vector3 oldVelocity;
    // New rigidbody velocity
    public Vector3 newVelocity;

    #endregion AARO

    public float CurrentVelocity
    {
        get
        {
            return rigidbody.velocity.magnitude;
        }
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        // Starting velocity when object is not moving
        oldVelocity = rigidbody.velocity;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        LevelManager.Instance.AddLevelBasketBall(this);

        rigidbodyStartPosition = rigidbody.position;
        AddSpin(Vector3.forward * CurrentVelocity * spinSpeed, ForceMode.Impulse);
    }

    private void Update()
    {
        // Kokeillaan Velocitya, paljonko arvo muuttuu heittäessä
        newVelocity = rigidbody.velocity;
        // Pallon liikkuessa pyöritetään palloa haluttuun suuntaan
        if (oldVelocity.magnitude < newVelocity.magnitude)
        {
            rigidbody.AddTorque(torqueX, torqueY, torqueZ, ForceMode.Force);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer.Equals(10))
        {
            LevelManager.Instance.UpdateScore(other.transform);
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

                //AudioManager.Instance.PlaySfx("BallBounce", rigidbody.position);
                AudioPlayer.Instance.PlaySfx(
                    audioSource,
                    "BallBounce");

                break;

            case 10:

                if (rigidbody.velocity.magnitude < minHitToSoundVelocity)
                {
                    Debug.LogWarning("HOOP");
                    AudioPlayer.Instance.PlaySfx(
                    audioSource,
                    "Sock");

                    return;
                }

                break;
        }
    }

    public void AddSpin(Vector3 spinDirection, ForceMode forceMode)
    {
        rigidbody.AddTorque(spinDirection, forceMode);
    }

    public void ResetPosition()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.position = rigidbodyStartPosition;
        rigidbody.rotation = Quaternion.Euler(Vector3.zero);
    }
}
