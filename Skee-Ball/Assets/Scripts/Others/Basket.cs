using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour {

    public GameObject basketball;
    public int points;
    public int highScore;

    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.GetComponent<Rigidbody>()) {
            basketball = other.gameObject;
        } else if (!other.gameObject.GetComponent<Rigidbody>()) {
            return;
        }
    }

    public void OnTriggerExit(Collider other) {
        basketball = null;
    }

    void Update() {
        if (basketball != null) {
            points++;
        }
    }
}
