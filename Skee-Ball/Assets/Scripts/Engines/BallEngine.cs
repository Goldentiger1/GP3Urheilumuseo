using UnityEngine;

public class BallEngine : MonoBehaviour
{
    private new Rigidbody rigidbody;
    private Vector3 closestPoint;
    private readonly float minHitToSoundVelocity = 1F;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
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
}
