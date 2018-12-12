using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTransformReset : MonoBehaviour {
    public Collider trigger;
    public GameObject controller;
    public float time;
    public List<GameObject> basketballs = new List<GameObject>();

    void Start() {
        trigger = this.gameObject.GetComponent<Collider>();
    }

    void OnTriggerStay(Collider other) {
        if(other.gameObject != null) {
            return;
        }
        controller = other.gameObject;
    }

    void Update() {
        if(controller != null) {
            time += Time.deltaTime;
        }if(time == 5) {

        }
    }
}
