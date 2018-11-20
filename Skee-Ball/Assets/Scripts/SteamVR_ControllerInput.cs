using UnityEngine;
using Valve.VR;

public class SteamVR_ControllerInput : MonoBehaviour
{
    [SteamVR_DefaultAction("Interact")]
    public SteamVR_Action_Boolean grabObj;

    public GameObject interactableObj;
    public Collider interactableCol;
    public Rigidbody controlAttachPoint;
    SteamVR_Behaviour_Pose followedObj;
    FixedJoint joint;

    void Awake() {
        followedObj = GetComponent<SteamVR_Behaviour_Pose>();
    }

    void OnTriggerEnter(Collider triggerCol) {

        if(joint == null && grabObj.GetStateDown(followedObj.inputSource)) {

            triggerCol = interactableCol;
            interactableObj.transform.position = controlAttachPoint.transform.position;
            joint = interactableObj.AddComponent<FixedJoint>();
            joint.connectedBody = controlAttachPoint;

            }

        else if(joint != null && grabObj.GetStateUp(followedObj.inputSource)){


            var tempRigidbody = joint.GetComponent<Rigidbody>();
            Object.DestroyImmediate(joint);
            joint = null;

            var origin = followedObj.origin ? followedObj.origin : followedObj.transform.parent;
            if(origin != null) {
                tempRigidbody.velocity = origin.TransformVector(followedObj.GetVelocity());
                tempRigidbody.angularVelocity = origin.TransformVector(followedObj.GetAngularVelocity());

            } else {
                tempRigidbody.velocity = followedObj.GetVelocity();
                tempRigidbody.angularVelocity = followedObj.GetAngularVelocity();
            }
            tempRigidbody.maxAngularVelocity = tempRigidbody.angularVelocity.magnitude;
        }
    }
}
