using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}

public class SimpleCarController : MonoBehaviour
{
    public List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    public float maxSteeringAngle;

    public bool grounded = false;
    public float groundedRaycastDistance = 0.2f;

    public float gravityMultiplier = -0.8f;

    public CheckPointSingle currentCheckpoint;

    /*
    float force = 0.5f;
    Vector3 airDirection;
    
    // car orientation
    private float carOrientationX;
    private float carOrientationY;
    private float carOrientationZ;

    // car rotation
    private float carRotationX;
    private float carRotationY;
    private float carRotationZ;*/

    public Rigidbody carRigidbody;

    public void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
        // maxMotorTorque = 300;
        // maxSteeringAngle = 30; 
    }

    // if currentCheckpoint changes return true


    public bool IsGrounded()
    {
        // This does not work, would be nice though.
        /*// if left whe isGrounded or right wheel isGrounded
        if (axleInfos[0].leftWheel.isGrounded || axleInfos[0].rightWheel.isGrounded)
        {
            // return true
            return true;
        }*/

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (Physics.Raycast(axleInfo.leftWheel.transform.position, -Vector3.up, groundedRaycastDistance))
            {
                return false;
            }
        }
        return true;
    }

    
    
    public void Brake()
    {
        if (Input.GetKey(KeyCode.S))
        {
            // brake
            foreach (AxleInfo axleInfo in axleInfos)
            {
                axleInfo.leftWheel.brakeTorque = 1000;
                axleInfo.rightWheel.brakeTorque = 1000;
            }
        }
        foreach (AxleInfo axleInfo in axleInfos)
        {
            axleInfo.leftWheel.brakeTorque = 0;
            axleInfo.rightWheel.brakeTorque = 0;
        }
    }

    // finds the corresponding visual wheel
    // correctly applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        Quaternion offsetRotation = new Quaternion(0.5f, 0.5f, 0f, 0f);

        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation * offsetRotation;
    }

    public void FixedUpdate()
    {
        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        /*
        grounded = IsGrounded();

        if (grounded)
        { }*/
        // Might be useful for drifting?
        Brake();

        foreach (AxleInfo axleInfo in axleInfos)
            {
                if (axleInfo.steering)
                {
                    axleInfo.leftWheel.steerAngle = steering;
                    axleInfo.rightWheel.steerAngle = steering;
                }
                if (axleInfo.motor)
                {
                    axleInfo.leftWheel.motorTorque = motor;
                    axleInfo.rightWheel.motorTorque = motor;
                }
                ApplyLocalPositionToVisuals(axleInfo.leftWheel);
                ApplyLocalPositionToVisuals(axleInfo.rightWheel);

            
            }

        // Used in the pitch rotation, if used then move to the function declation zone.
        // float roll = 0;

        // else
        // make the car glide if space is pressed.
        if (Input.GetKey(KeyCode.Space))
        {
            // Debug.Log("Space is pressed");
            // Attempt at making it work through pitch rotation, got annoyed, try again later.
            /*// Counteract gravity
            carRigidbody.AddForce(transform.up * 9.1f, ForceMode.Acceleration);
            carRigidbody.constraints = RigidbodyConstraints.FreezeRotationX;

            // Get roll angle from the steering wheel
            roll = -steering * 0.01f;

            // Get the car's current rotation
            carRotationX = transform.rotation.eulerAngles.x;
            carRotationY = transform.rotation.eulerAngles.y;

            carRotationX += roll;
            carRotationY += roll;

            // Apply the new rotation
            transform.rotation = Quaternion.Euler(carRotationX, carRotationY, 0);

            // Apply the new orientation
            transform.forward = new Vector3(carOrientationX, carOrientationY, 0);*/


            // Works but not great

            // Counteract gravity
            carRigidbody.AddForce(transform.up * 9.1f, ForceMode.Acceleration);
            // Freeze the z rotation so that the car does not roll forwards
            carRigidbody.constraints = RigidbodyConstraints.FreezeRotationX;

            /*// Dampen the car's rotation in the z axis, neither of these methods work well. 
            // carRigidbody.angularVelocity = new Vector3(0, 0, carRigidbody.angularVelocity.z * 0.9f);
            //carRigidbody.angularVelocity = Vector3.Lerp(carRigidbody.angularVelocity, Vector3.zero, Time.deltaTime * 3);*/

            // Make the car turn using steering
            carRigidbody.AddTorque(transform.up * steering * 0.05f, ForceMode.Acceleration);

            // Make the car accelerate in the direction it is facing
            carRigidbody.AddForce(transform.forward * motor * 0.02f, ForceMode.Acceleration);
        }


        if (Input.GetKey(KeyCode.LeftShift))
        {
            // Disactivated so that both shift and space bar are not pressed at the same time. 
            carRigidbody.AddForce(transform.up * 0.5f, ForceMode.Acceleration);
            
            // Make car spin like a beyblade
            carRigidbody.AddTorque(transform.up * steering * 0.2f, ForceMode.Impulse);

            // Make car move forwards.
            carRigidbody.AddForce(transform.forward * 2f, ForceMode.Acceleration);
        }
        // Unfreeeze rotation so the car can drive properly again.
        carRigidbody.constraints = RigidbodyConstraints.None;

    }
}