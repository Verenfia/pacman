using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MainMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI bestScore;
    [SerializeField] TextMeshProUGUI bestTime;
    GameManager instance;
    private void Start() {
        instance = GameManager.instance;
        instance.Load();
        bestScore.text = instance.LoadScore.ToString();
        TimeSpan timeSpan = TimeSpan.FromSeconds(instance.LoadTime);
        bestTime.text = string.Format("{0:D2}:{1:D2}:{2:D2}",
        timeSpan.Minutes,timeSpan.Seconds,timeSpan.Milliseconds);
    }

}
