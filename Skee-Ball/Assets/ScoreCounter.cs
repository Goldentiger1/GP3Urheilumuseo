using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ScoreCounter : MonoBehaviour {
    bool trigger1;
    bool trigger2;
    public TextMeshProUGUI uiScoreText;
    public TextMeshProUGUI uiScoreNumber;
    int score = 0;


    private void Update() {
        if (trigger1 && trigger2) {
            score += 2;
            uiScoreNumber.text = "" + score;
        }
    }

    private void OnTriggerStay(CapsuleCollider other) {
        if (other != null)
            trigger1 = true;
        else
            trigger1 = false;
    }

    private void OnTriggerEnter(SphereCollider other) {
        if (other != null)
            trigger2= true;
        else
            trigger2 = false;
    }

}
