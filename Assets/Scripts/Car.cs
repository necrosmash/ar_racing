using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField]
    private GameObject redCube;
    private GameObject redCubeGo;
    private Rigidbody rb;

    [SerializeField]
    private float speed = 1;

    private bool isPressed;

    void Start()
    {
        if (redCube == null) Debug.LogError("ctig rb is null");
        redCubeGo = GameObject.Instantiate(redCube, transform.position, transform.rotation);

        rb = GetComponent<Rigidbody>();
        if (rb == null) Debug.LogError("ctig rb is null");
    }

    // Update is called once per frame
    void Update()
    {
        redCubeGo.transform.position = transform.position + (transform.forward);

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            isPressed = true;
            Debug.Log("ctig isPressed became true");
        }

        else if (Input.touchCount == 0 && isPressed)
        {
            isPressed = false;
            Debug.Log("ctig isPressed became false");
        }
    }

    private void FixedUpdate()
    {
        if (isPressed)
        {
            Debug.Log("ctig isPressed remains true");
            //rb.velocity = transform.forward * speed;
            //rb.AddForce(Vector3.forward * speed);
            rb.AddForce(transform.forward * speed);
        }
    }
}
