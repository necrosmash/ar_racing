using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;
public class ImageDetecting : MonoBehaviour
{
    ARTrackedImageManager imgtracker;

    [SerializeField]
    private AudioSource spraySound;

    public Color carColor = Color.white;

    [SerializeField]
    private GameObject car;

    void Awake()
    {
        //Debug.Log("ctig17 in awake");
        imgtracker = GetComponent<ARTrackedImageManager>();
    }
    void OnEnable()
    {
        //Debug.Log("ctig17 onenable");
        imgtracker.trackedImagesChanged += myEventHandler;
    }
    void OnDisable()
    {
        //Debug.Log("ctig17 ondisable");
        imgtracker.trackedImagesChanged -= myEventHandler;
    }
    void myEventHandler(ARTrackedImagesChangedEventArgs eventArgs)
    {
        //Debug.Log("ctig17 myEventHandler");
        
        foreach (ARTrackedImage img in eventArgs.added)
        {
            handleTracking(img);
        }
        foreach (ARTrackedImage img in eventArgs.updated)
        {
            handleTracking(img);
        }
    }
    void handleTracking(ARTrackedImage img)
    {   
        string key;
        
        //Debug.Log("ctig17 handleTracking with " + img.referenceImage.name);
        /*if (img.trackingState == TrackingState.None)
        {
            Debug.Log("ctig17 img.trackingState null, returning");
            return;
        }*/

        key = img.referenceImage.name;

        Color car1 = Color.yellow;
        Color car2 = Color.red;
        Color car3 = Color.blue;
        Color car4 = Color.magenta;
        Color car5 = Color.green;

        //Debug.Log("ctig17 entering switch");
        switch (key)
        {
            case "car1":
                carColor = car1;
                break;
            case "car2":
                carColor = car2;
                break;
            case "car3":
                carColor = car3;
                break;
            case "car4":
                carColor = car4;
                break;
            case "car5":
                carColor = car5;
                break;
            default:
                carColor = Color.black;
                break;
        }
        
        //Debug.Log("ctig17 entering foreach child in transform");

        if (car == null) return;
        //Debug.Log("ctig17 car NOT null");
        car.transform.Find("Cube").GetComponent<MeshRenderer>().material.color = carColor;
        car.transform.Find("roof").GetComponent<MeshRenderer>().material.color = carColor;
        car.transform.Find("wings/center").GetComponent<MeshRenderer>().material.color = carColor;
        car.transform.Find("wings/left").GetComponent<MeshRenderer>().material.color = carColor;
        car.transform.Find("wings/right").GetComponent<MeshRenderer>().material.color = carColor;

        Debug.Log("ctig17 setting colour to " + carColor.ToString());
        if (!spraySound.isPlaying) spraySound.Play();

        /*foreach (Transform child in transform)
        {
            // get the mesh renderer of child
            mesh = child.GetComponent<MeshRenderer>();
            // if child has a mesh renderer
            if (mesh != null)
            {
                // render the colour to the cat
                mesh.material.color = carColor;
            }
        }

        Debug.Log("ctig17 entering foreach wings");
        // get the childern of the child called "wings"
        foreach (Transform child in transform.Find("wings"))
        {
            // get the mesh renderer of child
            mesh = child.GetComponent<MeshRenderer>();
            // if child has a mesh renderer
            if (mesh != null)
            {
                // render the colour to the cat
                mesh.material.color = carColor;
            }
        }*/

        //Debug.Log("ctig17 Found an image: " + img.referenceImage.name + " ("
           //+ img.trackingState + ")");
    }

    private void Update()
    {
        //if(car == null) Debug.LogError("ctig17 no car found");
    }
}

