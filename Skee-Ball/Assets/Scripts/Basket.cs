using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour {

    public GameObject hoop;
    public GameObject basketball;

    void Start() {
        hoop = GameObject.Find("Basketball hoop");
    }
    void OnTriggerEnter(Collider other) {
        if (!other.GetComponent<Rigidbody>()) {
            return;
        }
        basketball = other.gameObject;
    }

    void OnTriggerExit(Collider other) {
        
    }

}
