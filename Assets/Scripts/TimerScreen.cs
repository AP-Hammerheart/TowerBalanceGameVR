using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerScreen : MonoBehaviour
{
    public TextMeshPro timerText;
    public LevelController levelController;

    // Update is called once per frame
    void Update()
    {
        PrintTimer();
    }

    void PrintTimer()
    {
        string minutes = Mathf.Floor(levelController.Timer / 60).ToString("00");
        string seconds = (levelController.Timer % 60).ToString("00");
        string time = string.Format("{0}:{1}", minutes, seconds);
        timerText.text = time;
    }
}