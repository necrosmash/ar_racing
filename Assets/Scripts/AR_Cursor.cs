using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class AR_Cursor : MonoBehaviour
{
    [SerializeField]
    private GameObject track;
    
    private GameObject onScreenInput;

    //private Pose placementCursorPose;

    private ARRaycastManager raycastManager;

    private bool isPlacing; // whether or not we're currently placing a track

    private GameObject placeButtonGO;
    
    void Start()
    {
        if (track == null) Debug.LogError("track is null");
        raycastManager = GameObject.Find("AR Session Origin").GetComponent<ARRaycastManager>();
        placeButtonGO = gameObject.transform.Find("Canvas/Button").gameObject;
        onScreenInput = GameObject.Find("OnScreenInput");
        onScreenInput.SetActive(false);
        isPlacing = true;
    }

    void Update()
    {
        if (!isPlacing) return;
        if (raycastManager == null || track == null) return;
        
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var arRaycastHits = new List<ARRaycastHit>();
        raycastManager.Raycast(screenCenter, arRaycastHits, TrackableType.Planes);
        
        if (arRaycastHits.Count > 0)
        {
            track.transform.SetPositionAndRotation(arRaycastHits[0].pose.position, arRaycastHits[0].pose.rotation);
            Debug.Log("ctig10 checkpointholder pos" + 
                "x" + transform.Find("CheckPointHolder").transform.position.x +
            "y" + transform.Find("CheckPointHolder").transform.position.y +
            "z" + transform.Find("CheckPointHolder").transform.position.z);
            track.SetActive(true);
        }

        /*if (Input.touchCount > 0
            && Input.GetTouch(0).phase == TouchPhase.Began)
            //&& sphereGo == null)
        {
            Debug.Log("ctig Touch Detected");

            Vector2 touchPosition = Input.GetTouch(0).position;
            List<ARRaycastHit> hits = new List<ARRaycastHit>();

            if (raycastManager.Raycast(touchPosition, hits, TrackableType.Planes))
            {
                Debug.Log("ctig Raycast hit");
                Pose hitPose = hits[0].pose;
                GameObject.Instantiate(trackPrefab, hitPose.position, hitPose.rotation);
            }
        }*/
    }

    public void placeTrack()
    {
        isPlacing = false;
        placeButtonGO.SetActive(false);
        onScreenInput.SetActive(true);
    }
}
