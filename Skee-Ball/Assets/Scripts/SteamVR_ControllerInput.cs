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

    void Awake() {
        controllerInput = GetComponent<SteamVR_Behaviour_Pose>();
    }

    void OnTriggerEnter(Collider trigger) {
        if (!trigger.GetComponent<Rigidbody>()) {
            return;
        }
        interactableRig = trigger.GetComponent<Rigidbody>();
    }

    private void OnTriggerExit(Collider trigger) {
        interactableRig = null;
    }

    void FixedUpdate() {
        if(joint == null && grabObj.GetStateDown(controllerInput.inputSource)){
            if (interactableRig) {
                interactableObj = interactableRig.GetComponent<GameObject>();
                interactableObj.transform.position = controllerAttachPoint.transform.position;
                joint = interactableObj.AddComponent<FixedJoint>();
                joint.connectedBody = controllerAttachPoint;
            }
        }
        else if(joint != null && grabObj.GetStateUp(controllerInput.inputSource)) {
            if (interactableObj) {

            }
        }
    }


    /*
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
