using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour {

    public float timeRemaining = 30;
    [SerializeField] public bool timerOn = false;
    [SerializeField] public TMP_Text timerText = null;
    public static event Action timerFinished;

    void Update() {
        if (timerOn) {

            if (timeRemaining > 0) {

                timeRemaining -= Time.deltaTime;
                timerText.text = string.Format("{0:00}:{1:00}",(int)(timeRemaining/60),(int)(timeRemaining%60));

            }
            else { timeRemaining = 30; timerOn = false; timerFinished?.Invoke();}
        }
    }

    public void startTimer(Action func, int time) {
        timerFinished = func;
        timeRemaining = (float)time;
        timerOn = true;
    }

    public void startTimer(Action func) {
        timerFinished = func;
        timerOn = true;
    }

    public void startTimer(int time) {
        timeRemaining = (float)time;
        timerOn = true;
    }

    public void startTimer() {
        timerOn = true;
    }


}






