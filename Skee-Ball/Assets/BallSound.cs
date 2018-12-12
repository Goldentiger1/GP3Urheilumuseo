using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSound : MonoBehaviour {

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            print("Pallo pomppii");
            Fabric.EventManager.Instance.PostEvent("ballbounce");
        }
    }

    // Use this for initialization
    void Start () {
        Fabric.EventManager.Instance.PostEvent("street");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
