using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    public float groundedRaycastDistance = 0.5f;

    public float gravityMultiplier = -0.8f;

    public bool IsGrounded()
    {
        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (!Physics.Raycast(axleInfo.leftWheel.transform.position, -Vector3.up, groundedRaycastDistance))
            {
                return false;
            }
        }
        return true;
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

        grounded = IsGrounded();
        
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

        if (!grounded)
        {
            // Apply gravity multiplier to rigidbody
            //GetComponent<Rigidbody>().AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);

            //GetComponent<Rigidbody>().AddForce(transform.forward * motor, ForceMode.Acceleration);

            // steer left and right
            //GetComponent<Rigidbody>().AddTorque(transform.up * steering, ForceMode.Acceleration);
            //GetComponent<Rigidbody>().useGravity = false;
        }

    }
}