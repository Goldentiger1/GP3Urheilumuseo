using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public float timer;
    public float setVisibleTime;
    public GameObject button;

    private void Start() {
        button = gameObject.GetComponentInChildren<GameObject>();
    }

    void Update() {
        timer = Time.deltaTime;
        button.SetActive(false);
    }
}
