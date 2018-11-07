using UnityEngine;

public class BallEngine : MonoBehaviour
{
    private new Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        //rigidbody.maxAngularVelocity = Mathf.Infinity;
    }

    public void AddForce(Vector3 force, ForceMode forceMode)
    {
        rigidbody.AddForce(force, forceMode);
    }

    private void Update()
    {
        if(transform.position.y <= -20)
        {
            Destroy(gameObject);
        }
    }
}
