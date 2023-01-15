using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.MLAgents.Integrations.Match3;

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

    public CheckPointSingle currentCheckpoint, nextCheckpoint;

    //Boosting 
    private float timeStamp = 0f;
    public float boost;

    [SerializeField]
    private float boostCooldown = 5f;

    public float spinVal = 0.002f;

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
    private InputAsset controllerInput;

    public AudioSource driving;
    public AudioSource crash;
    public AudioSource skid;

    private int frameCounter = 5;
    private int frameLimit = 5;
    bool boostFlag = false;

    private void Awake()
    {
        controllerInput = new InputAsset();
    }

    private void OnEnable()
    {
        controllerInput.Enable();
    }

    private void OnDisable()
    {
        controllerInput.Disable();
    }

    public void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();

        transform.eulerAngles = new Vector3(
            transform.eulerAngles.x,
            GameObject.Find("Track").transform.Find("CheckPointHolder").transform.Find("CheckPoint 0(Clone)").transform.eulerAngles.y,
            transform.eulerAngles.z);

        carRigidbody.velocity = Vector3.zero;
        maxMotorTorque = 2.0f;
        maxSteeringAngle = 150;
        boost = 0.4f;
    }

    public float movement = 0.0f;
    Vector2 movementInput = Vector2.zero;
    float acceleration;
    float steering;

    public void Move()
    {

        movementInput = controllerInput.CarControllerAM.Move.ReadValue<Vector2>();
        acceleration = (maxMotorTorque * movementInput.y) * 0.005f;
        steering = maxSteeringAngle * movementInput.x;

        if (controllerInput.CarControllerAM.Boost.triggered && Time.time >= timeStamp)
        {
            timeStamp = Time.time + boostCooldown;
            frameCounter = 0;
        }
        if (frameCounter < frameLimit)
        {
            movement += boost;
            Debug.Log("Boosting!");
            frameCounter++;
        }

        // move the car
        movement = Mathf.Clamp(movement + acceleration, -maxMotorTorque, maxMotorTorque);

        if (movementInput.y == 0)
        {
            movement = Mathf.Lerp(movement, 0, 0.1f);
        }

        // Update car's position and rotation
        transform.position += transform.forward * movement * Time.deltaTime;

        // the smaller (maxMotorTorque * 10) is, the more steering will be dampened
        transform.Rotate(Vector3.up, Mathf.Clamp(Mathf.Lerp(steering, 0, movement / (maxMotorTorque * 25)), -maxSteeringAngle, maxSteeringAngle) * Time.deltaTime);

        // Drifting
        if (controllerInput.CarControllerAM.Spin.IsPressed())
        {
            if (!skid.isPlaying)
            {
                skid.Play();
            }
            movementInput = controllerInput.CarControllerAM.Move.ReadValue<Vector2>();

            transform.position += transform.right * steering * movement * -0.0035f * Time.deltaTime;
        }


        // Update wheel visuals
        foreach (AxleInfo axleInfo in axleInfos)
        {
            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }

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


    // Boosting
    /*private float Boost(float mov)
    {
        if (controllerInput.CarControllerAM.Boost.triggered && Time.time >= timeStamp)
        {
            timeStamp = Time.time + boostCooldown;

            mov += boost;
        }
        return mov;
    }*/

    /*public void Boost()
    {
        if (controllerInput.CarControllerAM.Boost.triggered && timeStamp <= Time.time)
        {  
            // Make car move forwards.
            carRigidbody.AddForce(transform.forward * boost, ForceMode.Acceleration);
            timeStamp = Time.time + boostCooldown;
            Debug.Log("BOOST ACTIVATED");
        }
    }*/

    public void Jump()
    {
        Vector2 movementInput = controllerInput.CarControllerAM.Move.ReadValue<Vector2>();
        float motor = maxMotorTorque * movementInput.y;
        float steering = maxSteeringAngle * movementInput.x;

        if (controllerInput.CarControllerAM.Jump.IsPressed())
        {
            {// Debug.Log("Space is pressed");
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
            }
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
        // Unfreeeze rotation so the car can drive properly again.
        carRigidbody.constraints = RigidbodyConstraints.None;
    }
    
    public void Drift()
    {
        if (controllerInput.CarControllerAM.Spin.IsPressed())
        {
            if (!skid.isPlaying)
            {
                skid.Play();
            }
            movementInput = controllerInput.CarControllerAM.Move.ReadValue<Vector2>();

            Vector3 sidewaysMovement = transform.right * movementInput.x * 1f;
            transform.position += sidewaysMovement * Time.deltaTime;
        }
    }

    public void Spin()
    {
        Vector2 movementInput = controllerInput.CarControllerAM.Move.ReadValue<Vector2>();
        float motor = maxMotorTorque * movementInput.y;
        float steering = maxSteeringAngle * movementInput.x;

        if (controllerInput.CarControllerAM.Spin.IsPressed())
        {
            if (!skid.isPlaying)
            {
                skid.Play();
            }
            // Disactivated so that both shift and space bar are not pressed at the same time. 
            carRigidbody.AddForce(transform.up * 0.5f * 5f, ForceMode.Acceleration);

            // Make car spin like a beyblade
            carRigidbody.AddTorque(transform.up * steering * spinVal, ForceMode.Impulse);

            // Make car move forwards.
            carRigidbody.AddForce(transform.forward * 2f, ForceMode.Acceleration);
        }
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

    private void Update()
    {
        if (nextCheckpoint == null && currentCheckpoint == null)
        {
            Debug.Log("ctig12 current checkpoint NULL");
            currentCheckpoint =
                GameObject.Find("Track").transform.Find("CheckPointHolder").transform.Find("CheckPoint 0(Clone)").GetComponent<CheckPointSingle>();

            nextCheckpoint =
                GameObject.Find("Track").transform.Find("CheckPointHolder").transform.Find("CheckPoint 1(Clone)").GetComponent<CheckPointSingle>();
        }
        if (boostFlag)
        {
            frameCounter++;
        }

        //Debug.Log("ctig12 current checkpoint: " + currentCheckpoint.name);


    }

    public void FixedUpdate()
    {


        /*
        grounded = IsGrounded();

        if (grounded)
        { }*/
        // Might be useful for drifting?
        Brake();

        // Activate using C or top button
        // Boost();

        // WASD or left stick
        Move();

        // Space or bottom button
        Jump();

        // LeftShift or right button
        // Spin();
        // Drift();
        // Used in the pitch rotation, if used then move to the function declation zone.
        // float roll = 0;

    }
}