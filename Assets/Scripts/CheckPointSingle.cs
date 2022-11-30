using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointSingle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Checkpoint triggered");
        if (other.gameObject.tag == "Car")
        // if(other.TryGetComponent(out Car car))
        {
            Debug.Log("Car has entered the checkpoint");
        }
    }
}
