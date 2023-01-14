using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public static float timer;
    public static bool timeStarted = false;
    public Text text;
    public Text bestText;

    private float minutes;
    private float seconds;

    private bool startedRace = false;
    private bool fullLap = false;
    private bool lapFinished = false;

    private int lapNr = 0;

    private float lapTime;
    private float bestTime = 6000f;

    private CheckPointSingle start;
    private SimpleCarController car;

    void Update()
    {
        Debug.Log(car.currentCheckpoint);
        if (car.currentCheckpoint == start)
        {
            startedRace = true;

            if (fullLap)
            {
                fullLap = false;
                lapFinished = true;
            }
        }

        if (startedRace)
        {
            timer += Time.deltaTime;
            lapTime += Time.deltaTime;
        }
        
        if (car.currentCheckpoint.NextCheckpoint == start)
        {
            fullLap = true;
        }

        if (lapFinished)
        {
            if (lapTime < bestTime)
            {
                bestTime = lapTime;
                lapTime = 0f;
            }

            lapFinished = false;
        }

        OnGUI();
    }

    private void Start()
    {
        start = GameObject.Find("Track").transform.Find("CheckPointHolder").transform.Find("CheckPoint 0(Clone)").GetComponent<CheckPointSingle>();
        car = GameObject.Find("Car").GetComponent<SimpleCarController>();
    }
    private void Awake()
    {
        timeStarted = true;
    }
    void OnGUI()
    {
        text.text = Mathf.Floor(timer / 60).ToString("00") + ":" + Mathf.FloorToInt(timer % 60).ToString("00");
        bestText.text = Mathf.Floor(bestTime/ 60).ToString("00") + ":" + Mathf.FloorToInt(bestTime % 60).ToString("00");

    }
}
