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
        if(other.gameObject != null) {
            return;
        }
        controller = other.gameObject;
    }

    void Update() {
        if(controller != null) {
            time += Time.deltaTime;
        }if(time >= buttonTime) {
            foreach(GameObject ball in basketballs) {
                for(int i = 0; i < ballTransforms.Count; i++) {
                    ball.transform.position = ballTransforms[i].position;
                    ball.transform.rotation = ballTransforms[i].rotation;
                }
            }
            time = 0;
        }
    }
}
