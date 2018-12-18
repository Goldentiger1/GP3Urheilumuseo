using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ScoreCounter : MonoBehaviour {

    public TextMeshProUGUI uiScoreText;
    public TextMeshProUGUI uiScoreNumber;
    public float SceneChangeTimer = 10f;

    int score = 0;

    private void Update() {
        //if (Input.GetKeyDown(KeyCode.U)) {
        //    UpdateScore();
        //}

        if(SceneChangeTimer <= 0) {
            ChangeScene();
        }

    }

    private void OnTriggerEnter(Collider other) {
        UpdateScore();
    }

    void UpdateScore() {
        score += 2;
        if (score < 10) {
            uiScoreNumber.text = "0" + score;
        }
        else {
            uiScoreNumber.text = "" + score;
            ChangeScene();
        }       
    }

    private void ChangeScene() {

    }

}
