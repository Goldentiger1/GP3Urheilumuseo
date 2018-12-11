using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SteamVR_ControllerInput : MonoBehaviour {

    public SteamVR_Action_Skeleton controller;
    public SteamVR_Action_Single triggerAnalog;
    public SteamVR_Action_Boolean trigger;    
    public SteamVR_Action_Boolean menuButton;
    public SteamVR_Action_Boolean dpadClick;
    public SteamVR_Action_Boolean dpadTouch;
    public SteamVR_Action_Vector2 dpadPosition;
    public SteamVR_Action_Boolean grip;
    public SteamVR_Action_Out vibration;
}