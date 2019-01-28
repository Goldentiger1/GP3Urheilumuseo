using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

    private float timer;
    private int scene = 0;
    public float sceneSwitchTime;


    void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    void Update() {
        timer += Time.deltaTime;
        if(timer >= sceneSwitchTime) {
            SceneManager.LoadScene(sceneBuildIndex:scene += 1);
            timer -= timer;
        } 
    }
}
