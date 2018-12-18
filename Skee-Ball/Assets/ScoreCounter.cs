using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreCounter : MonoBehaviour {

    public TextMeshProUGUI uiScoreText;
    public TextMeshProUGUI uiScoreNumber;
    public float SceneChangeTimer = 60f;
    public Vector3 throwStart;
    public float throwDistance;
    public float throwDistanceRequiredForThreePoints = 7f;
    private readonly float sceneChangeWaitTime = 2f;

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
        print(throwDistance);
        print(transform.position);
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

        int currentScene = SceneManager.GetActiveScene().buildIndex;

        switch (currentScene) {

            case 1:

                SceneManager.LoadScene(2);

                break;

            case 2:

                SceneManager.LoadScene(0);

                break;

            default:

                break;
        }
    }
}
