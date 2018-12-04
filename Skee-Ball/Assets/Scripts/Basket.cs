using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour {

    public GameObject hoop;
    public GameObject basketball;
    public int points;

    void Start() {
        hoop = GameObject.Find("Basketball hoop");
    }
    void OnTriggerEnter(Collider other) {
        if (!other.GetComponent<Rigidbody>()) {
            return;
        }
        basketball = other.gameObject;
        scoreAdd(points);
    }

    void OnTriggerExit(Collider other) {
        basketball = null;
    }

    public void scoreAdd(int score) {
        score++;
    }
}
