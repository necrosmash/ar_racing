using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointSingle : MonoBehaviour
{
    private CheckPointSingle nextCheckpoint;

    public CheckPointSingle NextCheckpoint
    {
        get { return nextCheckpoint; }
        set { nextCheckpoint = value; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Car")
        {
            // currentCheckpoint is actually the current checkpoint the car needs to go through not the last one it went through.
            if (other.gameObject.GetComponent<SimpleCarController>().currentCheckpoint == this || other.gameObject.GetComponent<SimpleCarController>().currentCheckpoint == null)
            {
                // set the car's current checkpoint to the next checkpoint
                other.gameObject.GetComponent<SimpleCarController>().currentCheckpoint = nextCheckpoint;
                Debug.Log("Car through checkpoint " + this.name);
                Debug.Log("Cars' next checkpoint is " + nextCheckpoint);
            }
            /*if (other.gameObject.GetComponent<SimpleCarController>().flag == true)
            {
                other.gameObject.GetComponent<SimpleCarController>().currentCheckpoint = this;
            }*/
        
            else
            {
                Debug.Log("Car is going in the wrong direction!");
                return;
            }


            // Debug.Log("Car has entered the checkpoint " + gameObject.name);
            // ChangeColor(Color.blue);

            // other.gameObject.GetComponent<SimpleCarController>().currentCheckpoint = this;
        }
    }

   /* // Change the checkpoints color
    public void ChangeColor(Color color)
    {
        gameObject.GetComponent<Material>().color = color;
    }*/
}
