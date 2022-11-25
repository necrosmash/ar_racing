using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class AR_Cursor : MonoBehaviour
{
    [SerializeField]
    private GameObject carPrefab;

    [SerializeField]
    private ARRaycastManager raycastManager;
    
    void Start()
    {
        if (carPrefab == null) Debug.LogError("carPrefab is null");
    }

    void Update()
    {
        if (Input.touchCount > 0
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
                GameObject.Instantiate(carPrefab, hitPose.position, hitPose.rotation);
            }
        }
    }
}
