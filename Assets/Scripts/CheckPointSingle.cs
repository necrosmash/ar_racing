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
            Debug.Log("Car has entered the checkpoint " + gameObject.name);
            // ChangeColor(Color.blue);
 
            other.gameObject.GetComponent<SimpleCarController>().currentCheckpoint = this;

            Debug.Log("Cars' next checkpoint is " + nextCheckpoint);
        }
    }

   /* // Change the checkpoints color
    public void ChangeColor(Color color)
    {
        gameObject.GetComponent<Material>().color = color;
    }*/
}
