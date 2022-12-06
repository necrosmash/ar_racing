using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TapToPlace : MonoBehaviour
{
    [SerializeField]
    private GameObject carPrefab;
    private GameObject carInstance;

    [SerializeField]
    private GameObject pathCreatorPrefab;
    private GameObject pathCreatorInstance;

    //[SerializeField]
    //private GameObject checkpointHolderPrefab;
    //private GameObject checkpointHolderInstance;

    [SerializeField]
    private Camera myCamera;

    private ARRaycastManager rays;
    private ARAnchorManager anc;
    private ARPlaneManager plan;

    void Start()
    {
        if (carPrefab == null) Debug.LogError("carPrefab is null");

        rays = this.gameObject.GetComponent<ARRaycastManager>();
        anc = this.gameObject.GetComponent<ARAnchorManager>();
        plan = this.gameObject.GetComponent<ARPlaneManager>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Debug.Log("ctig3 touch count > 0");
            if (pathCreatorInstance == null) instantiatePathCreator();
            else if (carInstance == null) instantiateCar();
        }

        {
            if (Input.touchCount > 0
                && Input.GetTouch(0).phase == TouchPhase.Began)
            //&& sphereGo == null)
            {
                Debug.Log("ctig Touch Detected");

                Vector2 touchPosition = Input.GetTouch(0).position;
                List<ARRaycastHit> hits = new List<ARRaycastHit>();

                //if (raycastManager.Raycast(touchPosition, hits, TrackableType.Planes))
                {
                    Debug.Log("ctig Raycast hit");
                    Pose hitPose = hits[0].pose;
                    GameObject.Instantiate(carPrefab, hitPose.position, hitPose.rotation);
                }
            }
        }
    }

    private void instantiateCar()
    {
        var hitInfo = getHitInfo();
        
        // check if we hit the actual track now?
    }

    private void instantiatePathCreator()
    {
        Debug.Log("ctig3 instantiatePathCreator");
        var hitInfo = getHitInfo();

        Debug.Log("ctig3 hitInfo: " + hitInfo);

        if (hitInfo.hit == true)
        {
            Debug.Log("ctig3 hitInfo == true");

            ARPlane plane;
            ARAnchor point;
            ARRaycastHit nearest = hitInfo.myHits[0];
            pathCreatorInstance = Instantiate(pathCreatorPrefab, nearest.pose.position, nearest.pose.rotation);

            Debug.Log("ctig3 instantiated pathCreatorInstance");

            plane = plan.GetPlane(nearest.trackableId);

            if (plane != null)
            {
                Debug.Log("ctig3 plane != null");
                point = anc.AttachAnchor(plane, nearest.pose);
                Debug.Log("Added an anchor to a plane " + nearest);
            }
            else
            {
                Debug.Log("ctig3 plane == null");
                point = anc.AddAnchor(nearest.pose); //Natasha: is obsolete but we can ignore this for now
                Debug.Log("Added another anchor " + nearest);
            }

            Debug.Log("ctig3 setting pathCreatorInstance.transform.parent to point.transform");
            pathCreatorInstance.transform.parent = point.transform;
        }
    }

    private (bool hit, List<ARRaycastHit> myHits) getHitInfo()
    {
        Debug.Log("ctig3 getHitInfo");

        List<ARRaycastHit> myHits = new List<ARRaycastHit>();
        Vector3 screenCenter = myCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));

        bool hit = rays.Raycast(screenCenter, myHits, TrackableType.FeaturePoint | TrackableType.PlaneWithinPolygon);

        Debug.Log("ctig3 returning hitInfo");
        return (hit, myHits);
    }
}
