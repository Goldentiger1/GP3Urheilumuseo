using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ScoreCounter : MonoBehaviour {

    public TextMeshProUGUI uiScoreText;
    public TextMeshProUGUI uiScoreNumber;
    public float SceneChangeTimer = 10f;
    public Vector3 throwStart;
    public float throwDistance;
    public float throwDistanceRequiredForThreePoints = 7f;
    private float sceneChangeWaitTime = 2f;

    int score = 0;

    private void Update() {
        //if (Input.GetKeyDown(KeyCode.U)) {
        //    UpdateScore();
        //}

        SceneChangeTimer -= Time.deltaTime;
        if(SceneChangeTimer <= 0) {
            ChangeScene();
        }

    }

    private void OnTriggerEnter(Collider other) {
        UpdateScore();
    }

    void UpdateScore() {
        
        throwDistance = Vector3.Distance(throwStart, transform.position);
        if (throwDistance > throwDistanceRequiredForThreePoints) {
            score += 3;
        } else {
            score += 2;
        }

        if (score < 10) {

            uiScoreNumber.text = "0" + score;
        } else {
            uiScoreNumber.text = "" + score;
            Invoke( "ChangeScene", sceneChangeWaitTime);
        }
    }
    

    private void ChangeScene() {

    }

}
