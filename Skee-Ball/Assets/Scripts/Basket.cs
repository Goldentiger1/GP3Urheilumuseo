using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour {

    public GameObject hoop;
    public GameObject basketball;

    void Start() {
        hoop = GameObject.Find("Basketball hoop");
        
    }
}
