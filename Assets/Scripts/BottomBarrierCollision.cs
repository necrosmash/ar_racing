using PathCreation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomBarrierCollision : MonoBehaviour
{
    private CheckPointSingle currentCheckpoint;
    private Rigidbody carRigidbody;

    // get path
    public PathCreator pathCreator;

    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Car")
        {
            Debug.Log("Car has hit the lower bounds");
            // get the car's current checkpoint
            currentCheckpoint = other.gameObject.GetComponent<SimpleCarController>().currentCheckpoint;

            // make the car have the right way up
            carRigidbody = other.gameObject.GetComponent<Rigidbody>();
            carRigidbody.rotation = Quaternion.Euler(0, 0, 0);
            
            

            // position the car at the current checkpoint
            other.gameObject.transform.position = currentCheckpoint.transform.position;              

            // other.gameObject.transform.position = new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y + 10f, other.gameObject.transform.position.z);

            // for one second SpinCar
            StartCoroutine(SpinCar(other.gameObject));

        }
    }
    // Create IEnumerator to spin car for one second
    IEnumerator SpinCar(GameObject car)
    {
        carRigidbody = car.GetComponent<Rigidbody>();
        carRigidbody.AddForce(transform.up * 1f, ForceMode.Acceleration);
        carRigidbody.AddTorque(transform.up * 40f, ForceMode.Impulse);
        yield return new WaitForSeconds(10);
    }

}
