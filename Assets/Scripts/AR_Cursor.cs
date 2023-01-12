using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class AR_Cursor : MonoBehaviour
{
    [SerializeField]
    private GameObject trackPrefab;
    private GameObject track;

    //private Pose placementCursorPose;

    [SerializeField]
    private ARRaycastManager raycastManager;
    
    void Start()
    {
        if (trackPrefab == null) Debug.LogError("carPrefab is null");
        else track = Instantiate(trackPrefab);
        raycastManager = GameObject.Find("AR Session Origin").GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (raycastManager == null || track == null) return;
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var arRaycastHits = new List<ARRaycastHit>();
        raycastManager.Raycast(screenCenter, arRaycastHits, TrackableType.Planes);
        
        if(arRaycastHits.Count > 0)
        {
            track.transform.SetPositionAndRotation(arRaycastHits[0].pose.position, arRaycastHits[0].pose.rotation);
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
}
