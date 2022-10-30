using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class HUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI elapsedTime;
    [SerializeField] TextMeshProUGUI ghostScaredTime;
    [SerializeField] GameObject ghostTimer;

    float time;
    GameManager instance;
    private void Start() {
        time = 0;
        instance = GameManager.instance;
        ghostTimer.gameObject.SetActive(false);
    }
    public void Update(){
        time+=Time.deltaTime;
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        elapsedTime.text = string.Format("{0:D2}:{1:D2}:{2:D2}",
        timeSpan.Minutes,timeSpan.Seconds,timeSpan.Milliseconds);
        scoreText.text = instance.TotalScore.ToString();
        instance.SetGameTime(time);
    }
}
