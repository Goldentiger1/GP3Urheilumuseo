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

        }
    }
}