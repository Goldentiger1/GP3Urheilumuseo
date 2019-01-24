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
        oldVelocity = rigidbody.velocity;
    }

    private void Start()
    {
        rigidbodyStartPosition = rigidbody.position;

        AddTorque(Vector3.forward * spinSpeed, ForceMode.Impulse);
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

                //Debug.LogError("Hit");

                if (rigidbody.velocity.magnitude < minHitToSoundVelocity)
                {
                    // Debug.LogError("Not enough velocity: " + rigidbody.velocity);
                    return;
                }

                Fabric.EventManager.Instance.PostEvent("ballbounce");

                // Debug.LogError("Bounce sound");

                break;

            case 10:

                if (rigidbody.velocity.magnitude < minHitToSoundVelocity)
                {
                    return;
                }

                //AudioManager.Instance.PlayClipAtPoint("BackBoard", transform.position);


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
