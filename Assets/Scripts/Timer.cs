using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public static float timer;
    public static bool timeStarted = false;
    public Text text;

    private float minutes;
    private float seconds;


    void Update()
    {
        if (timeStarted == true)
        {
            timer += Time.deltaTime;
        }


        OnGUI();
    }

    private void Awake()
    {
        timeStarted = true;
    }
    void OnGUI()
    {
        minutes = Mathf.Floor(timer / 60);
        seconds = timer % 60;

        /*
        string minutesT;
        string secondsT;
        if (minutes < 10)
        {
            minutesT = "0" + minutes.ToString();
        }
        else
        {
            minutesT =  minutes.ToString();
        }
        if (seconds < 10)
        {
            secondsT = "0" + Mathf.RoundToInt(seconds).ToString();
        }
        else
        {
            secondsT = Mathf.RoundToInt(seconds).ToString();
        }


            text.text = minutesT + ":" + secondsT;
        */
        text.text = Mathf.Floor(timer / 60).ToString("00") + ":" + Mathf.FloorToInt(timer % 60).ToString("00");

    }
}
