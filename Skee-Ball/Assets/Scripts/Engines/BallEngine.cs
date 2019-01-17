using UnityEngine;

public class BallEngine : MonoBehaviour
{
    private new Rigidbody rigidbody;
    private readonly string ScoreTriggerTag = "ScoreTrigger";
    private Vector3 closestPoint;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void AddForce(Vector3 force, ForceMode forceMode)
    {
        rigidbody.AddForce(force, forceMode);
    }

    public void AddTorque(Vector3 torque, ForceMode forceMode)
    {
        rigidbody.AddTorque(torque, forceMode);
    }

    private void Update()
    {
        if(transform.position.y <= -20)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {     
        if (other.CompareTag(ScoreTriggerTag))
        {
            closestPoint = other.ClosestPoint(transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(ScoreTriggerTag))
        {
            if(closestPoint.y > transform.position.y)
            {
                LevelManager.Instance.AddScore(2);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.layer)
        {
            case 9:

                if (rigidbody.velocity.magnitude < 2)
                {
                    return;
                }

                //AudioManager.Instance.PlayClipAtPoint("Bounce", transform.position);

                break;

            case 10:

                if (rigidbody.velocity.magnitude < 2)
                {
                    return;
                }

                //AudioManager.Instance.PlayClipAtPoint("BackBoard", transform.position);
                

                break;
        }
    }
}
