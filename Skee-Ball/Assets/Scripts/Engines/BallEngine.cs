using UnityEngine;

public class BallEngine : MonoBehaviour
{
    private new Rigidbody rigidbody;
    private Vector3 rigidbodyStartPosition;
    private readonly float minHitToSoundVelocity = 1F;
    private readonly float spinSpeed = 10f;

    #region AARO

    // World global position
    private GameObject world;
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
        // World local space
        world = GameObject.FindGameObjectWithTag("World").gameObject;
    }

    private void Start()
    {
        rigidbodyStartPosition = rigidbody.position;

        // AddTorque(Vector3.forward * CurrentVelocity * spinSpeed, ForceMode.Impulse);
    }

    private void Update()
    {
        // Kokeillaan Velocitya, paljonko arvo muuttuu heittäessä
        newVelocity = rigidbody.velocity;
        if (oldVelocity.magnitude < newVelocity.magnitude)
        {

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
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

                AudioManager.Instance.PlaySfx("Ballbounce");

                break;

            case 10:

                if (rigidbody.velocity.magnitude < minHitToSoundVelocity)
                {
                    return;
                }

                break;
        }
    }

    public void AddTorque(Vector3 newTorque, ForceMode forceMode)
    {
        rigidbody.AddTorque(newTorque, forceMode);
    }

    public void ResetPosition()
    {
        rigidbody.position = rigidbodyStartPosition;
    }
}
