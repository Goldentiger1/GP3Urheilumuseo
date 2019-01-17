using UnityEngine;
using UnityEngine.XR; //needs to be UnityEngine.VR in version before 2017.2

public class HandGrabbing : MonoBehaviour
{
    public string InputName;
    public XRNode NodeType;
    public Vector3 ObjectGrabOffset;
    public float GrabDistance = 0.1f;
    public string GrabTag = "Grab";
    public float ThrowMultiplier = 1.5f;

    private Transform currentObject;
    private Vector3 lastFramePosition;

    private void Start()
    {
        currentObject = null;
        lastFramePosition = transform.position;
    }

    private void Update()
    {
        transform.localPosition = InputTracking.GetLocalPosition(NodeType);
        transform.localRotation = InputTracking.GetLocalRotation(NodeType);

        if (currentObject == null)
        {
            //check for colliders in proximity
            Collider[] colliders = Physics.OverlapSphere(transform.position, GrabDistance);
            if (colliders.Length > 0)
            {
                //if there are colliders, take the first one if we press the grab button and it has the tag for grabbing
                if (Input.GetAxis(InputName) >= 0.01f && colliders[0].transform.CompareTag(GrabTag))
                {
                    //set current object to the object we have picked up
                    currentObject = colliders[0].transform;

                    //if there is no rigidbody to the grabbed object attached, add one
                    if (currentObject.GetComponent<Rigidbody>() == null)
                    {
                        currentObject.gameObject.AddComponent<Rigidbody>();
                    }

                    //set grab object to kinematic (disable physics)
                    currentObject.GetComponent<Rigidbody>().isKinematic = true;
                }
            }
        }
        else
        //we have object in hand, update its position with the current hand position (+defined offset from it)
        {
            currentObject.position = transform.position + ObjectGrabOffset;

            //if we we release grab button, release current object
            if (Input.GetAxis(InputName) < 0.01f)
            {
                //set grab object to non-kinematic (enable physics)
                Rigidbody objectRGB = currentObject.GetComponent<Rigidbody>();
                objectRGB.isKinematic = false;

                //calculate the hand's current velocity
                Vector3 CurrentVelocity = (transform.position - lastFramePosition) / Time.deltaTime;

                //set the grabbed object's velocity to the current velocity of the hand
                objectRGB.velocity = CurrentVelocity * ThrowMultiplier;

                //release the reference
                currentObject = null;
            }
        }

        //save the current position for calculation of velocity in next frame
        lastFramePosition = transform.position;
    }
}