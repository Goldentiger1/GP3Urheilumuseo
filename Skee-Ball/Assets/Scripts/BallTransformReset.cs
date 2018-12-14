using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTransformReset : MonoBehaviour {
    public Collider trigger;
    public GameObject controller;
    public float time;
    public float buttonTime;
    public List<GameObject> basketballs = new List<GameObject>();
    public List<Transform> ballTransforms = new List<Transform>();

    void Start() {
        trigger = this.gameObject.GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other) {
        controller = other.gameObject;
    }

    void Update() {
        if (controller != null) {
            time += Time.deltaTime;
        }
        if (time >= buttonTime) {
            ballPos(ballTransforms, basketballs);
            time -= time;
            controller = null;
        }
    }

    void ballPos(List<Transform> bT, List<GameObject> b) {
        for (int i = 0; i < b.Count; i++) {
            //if (b[i].transform != bT[i].transform) {
            b[i].GetComponent<Rigidbody>().position = bT[i].position;
                //b[i].transform.position = bT[i].transform.position;
                
           // }
        }
    }
}