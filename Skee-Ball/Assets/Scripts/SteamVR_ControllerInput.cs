using UnityEngine;
using Valve.VR;

public class SteamVR_ControllerInput : MonoBehaviour
{
    [SteamVR_DefaultAction("Interact")]
    public SteamVR_Action_Boolean grabObj;

    // OntriggerEnter ja -Exit muuttuja
    public Rigidbody interactableRig;

    // Vuorovaikutettavan esineen muuttuja
    public GameObject interactableObj;

    // Vuorovaikutettavaan esineen sijaintiin vaikuttava muuttuja ohjaimessa
    public Rigidbody controllerAttachPoint;

    // Steamin oma Pose funktio
    SteamVR_Behaviour_Pose controllerInput;

    FixedJoint joint;

    /*


    void Awake() {
        followedObj = GetComponent<SteamVR_Behaviour_Pose>();
    }

    void OnTriggerEnter(Collider triggerCol) {
        if (!triggerCol.GetComponent<Rigidbody>()) {
            return;
        }
        collidingObj = triggerCol.gameObject;        
    }
    void OnTriggerExit(Collider triggerCol) {
        collidingObj = null;
    }

    void FixedUpdate() {
        if(joint == null && grabObj.GetStateDown(followedObj.inputSource)) {
            // Tähän if(collidingObj){
            interactableObj.transform.position = controlAttachPoint.transform.position;
            joint = interactableObj.AddComponent<FixedJoint>();
            joint.connectedBody = controlAttachPoint;
        }
        else if(joint != null && grabObj.GetStateUp(followedObj.inputSource)) {
            // Tähän if(ObjectinHand){
            var tempRigidbody = joint.GetComponent<Rigidbody>();
            Object.DestroyImmediate(joint);
            joint = null;

            var origin = followedObj.origin ? followedObj.origin : followedObj.transform.parent;

            if(origin != null) {
                tempRigidbody.velocity = origin.TransformVector(followedObj.GetVelocity());
                tempRigidbody.angularVelocity = origin.TransformVector(followedObj.GetAngularVelocity());
            }
            else {
                tempRigidbody.velocity = followedObj.GetVelocity();
                tempRigidbody.angularVelocity = followedObj.GetAngularVelocity();
            }
            tempRigidbody.maxAngularVelocity = tempRigidbody.angularVelocity.magnitude;
        }
    }
    */
}
