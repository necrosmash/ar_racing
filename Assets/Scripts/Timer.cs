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

    private float lapTime = 0f;
    private float bestTime = 600f;

    private CheckPointSingle start;
    private SimpleCarController car;

    void Update()
    {
        if (car == null)
        {
            // for BlankAR
            if (GameObject.Find("Car") != null && GameObject.Find("Car").GetComponent<SimpleCarController>() != null)
                car = GameObject.Find("Car").GetComponent<SimpleCarController>();
            
            // for TrackScene
            if (GameObject.Find("Car(Clone)") != null && GameObject.Find("Car(Clone)").GetComponent<SimpleCarController>() != null)
                car = GameObject.Find("Car(Clone)").GetComponent<SimpleCarController>();
        }

        if (start == null)
        {
            start = GameObject.Find("Track").transform.Find("CheckPointHolder").transform.Find("CheckPoint 1(Clone)").GetComponent<CheckPointSingle>();
        }

        if (car == null || start == null)
        {
            return;
        }


        //Debug.Log(car.currentCheckpoint);

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

        if (car.currentCheckpoint.NextCheckpoint == start && startedRace)
        {
            Debug.Log("full lap!");
            fullLap = true;
        }

        if (lapFinished)
        {
            if (lapTime < bestTime)
            {
                bestTime = lapTime;
            }

            lapTime = 0f;
            timer = 0f;
            lapFinished = false;
        }


        OnGUI();

    }

    private void Start()
    {


    }
    private void Awake()
    {
        timeStarted = true;
    }
    void OnGUI()
    {
        text.text = Mathf.Floor(timer / 60).ToString("00") + ":" + Mathf.FloorToInt(timer % 60).ToString("00");
        bestText.text = "Best lap time " + Mathf.Floor(bestTime / 60).ToString("00") + ":" + Mathf.FloorToInt(bestTime % 60).ToString("00");

    }
}