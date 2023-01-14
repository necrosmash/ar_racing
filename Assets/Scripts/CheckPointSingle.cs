using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPointSingle : MonoBehaviour
{
    private CheckPointSingle nextCheckpoint;
    public Text errorMessage;

    public CheckPointSingle NextCheckpoint
    {
        get { return nextCheckpoint; }
        set { nextCheckpoint = value; }
    }

    public void Start()
    {
        errorMessage = GameObject.Find("OnScreenInput").transform.Find("errorText").gameObject.GetComponent<Text>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Car")
        {
            // currentCheckpoint is actually the current checkpoint the car needs to go through not the last one it went through.
            if (other.gameObject.GetComponent<SimpleCarController>().nextCheckpoint == this)
            {
                if (errorMessage != null) errorMessage.enabled = false;
                // set the car's current checkpoint to the next checkpoint
                other.gameObject.GetComponent<SimpleCarController>().nextCheckpoint = nextCheckpoint;
                other.gameObject.GetComponent<SimpleCarController>().currentCheckpoint = this;
            }
            /*if (other.gameObject.GetComponent<SimpleCarController>().flag == true)
            {
                other.gameObject.GetComponent<SimpleCarController>().currentCheckpoint = this;
            }*/
        
            else if (other.gameObject.GetComponent<SimpleCarController>().nextCheckpoint != this &&
                other.gameObject.GetComponent<SimpleCarController>().currentCheckpoint != this &&
                other.gameObject.GetComponent<SimpleCarController>().currentCheckpoint != null)
            {
                if (errorMessage != null) errorMessage.enabled = true;
                Debug.Log("ctig16 Car is going in the wrong direction!");
                return;
            }


            // Debug.Log("Car has entered the checkpoint " + gameObject.name);
            // ChangeColor(Color.blue);

            // other.gameObject.GetComponent<SimpleCarController>().currentCheckpoint = this;
        }
    }

    private void Update()
    {
        if (errorMessage == null)
            errorMessage = GameObject.Find("OnScreenInput").transform.Find("errorText").gameObject.GetComponent<Text>();
        
        //Debug.Log("ctig16" + gameObject.name + " -> " + nextCheckpoint.gameObject.name);
    }

    /* // Change the checkpoints color
     public void ChangeColor(Color color)
     {
         gameObject.GetComponent<Material>().color = color;
     }*/
}
