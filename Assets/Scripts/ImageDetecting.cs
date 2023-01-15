using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;
public class ImageDetecting : MonoBehaviour
{
    ARTrackedImageManager imgtracker;

    public Color carColor = Color.white;
    private MeshRenderer mesh;

    void Awake()
    {
        imgtracker = GetComponent<ARTrackedImageManager>();
    }
    void OnEnable()
    {
        imgtracker.trackedImagesChanged += myEventHandler;
    }
    void OnDisable()
    {
        imgtracker.trackedImagesChanged -= myEventHandler;
    }
    void myEventHandler(ARTrackedImagesChangedEventArgs eventArgs)
    {

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
        if (img.trackingState == TrackingState.None)
        {
            return;
        }

        key = img.referenceImage.name;

        Color car1 = Color.yellow;
        Color car2 = Color.red;
        Color car3 = Color.blue;
        Color car4 = Color.magenta;
        Color car5 = Color.green;

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
        
        foreach (Transform child in transform)
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
        }

        Debug.Log("Found an image: " + img.referenceImage.name + " ("
           + img.trackingState + ")");
    }

    private void Update()
    {
        if(mesh == null)
        {
            mesh = GameObject.Find("Car(Clone)").transform.GetComponent<MeshRenderer>();
        }
    }
}

