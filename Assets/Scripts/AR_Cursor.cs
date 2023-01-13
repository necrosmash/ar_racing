using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class AR_Cursor : MonoBehaviour
{
    [SerializeField]
    private GameObject track;

    [SerializeField]
    private GameObject carPrefab;
    private GameObject carGO;
    
    private GameObject onScreenInput;
    private GameObject placeButtonGO;
    
    [SerializeField]
    private GameObject roadMeshHolder;

    private ARRaycastManager raycastManager;
    private Camera arCamera;
    
    private int placementIdx; // whether or not we're currently placing a track
    private MeshRenderer roadMesh;

    private ARPlaneManager planeManager;
    private ARAnchorManager anchorManager;

    ARRaycastHit currentNearestHit;

    void Start()
    {
        roadMesh = roadMeshHolder.GetComponent<MeshRenderer>();
        roadMesh.enabled = false;

        if (track == null) Debug.LogError("track is null");
        raycastManager = GameObject.Find("AR Session Origin").GetComponent<ARRaycastManager>();
        arCamera = GameObject.Find("AR Session Origin").transform.Find("AR Camera").GetComponent<Camera>();
        placeButtonGO = gameObject.transform.Find("Canvas/Button").gameObject;
        onScreenInput = GameObject.Find("OnScreenInput");
        planeManager = GameObject.Find("AR Session Origin").GetComponent<ARPlaneManager>();
        anchorManager = GameObject.Find("AR Session Origin").GetComponent<ARAnchorManager>();

        if (track == null) Debug.LogError("ctig10 track is null");
        if (raycastManager == null) Debug.LogError("ctig10 raycastManager is null");
        if (arCamera == null) Debug.LogError("ctig10 arCamera is null");
        if (placeButtonGO == null) Debug.LogError("ctig10 placeButtonGO is null");
        if (onScreenInput == null) Debug.LogError("ctig10 onScreenInput is null");
        if (roadMeshHolder == null) Debug.LogError("ctig10 roadMeshHolder is null");
        if (planeManager == null) Debug.LogError("ctig10 planeManager is null");
        if (anchorManager == null) Debug.LogError("ctig10 anchorManager is null");

        onScreenInput.SetActive(false);
        placementIdx = 0;
    }

    void Update()
    {
        Debug.Log("ctig11 y difference: " +
            (track.transform.Find("Path Creator").transform.position.y - track.transform.Find("CheckPointHolder").transform.position.y).ToString());

        if (placementIdx >= 2) return;
        if (raycastManager == null || track == null) return;
        
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var arRaycastHits = new List<ARRaycastHit>();
        bool isHit = raycastManager.Raycast(screenCenter, arRaycastHits, TrackableType.Planes);

        if (isHit && placementIdx == 0 && arRaycastHits.Count > 0)
        {
            currentNearestHit = arRaycastHits[0];
            track.transform.SetPositionAndRotation(currentNearestHit.pose.position, currentNearestHit.pose.rotation);
            Debug.Log("ctig10 setting track to active");

            roadMesh.enabled = true;

            //track.SetActive(true);
            /*Debug.Log("ctig10 checkpointholder pos" + 
                "x" + GameObject.Find("Track").transform.Find("CheckPointHolder").position.x +
            "y" + GameObject.Find("Track").transform.Find("CheckPointHolder").position.y +
            "z" + GameObject.Find("Track").transform.Find("CheckPointHolder").position.z);*/
        }

        if (placementIdx == 1)
        {
            Vector2 touchPosition = Input.GetTouch(0).position;

            List<RaycastHit> hits = new List<RaycastHit>();
            hits.AddRange(Physics.RaycastAll(arCamera.ScreenPointToRay(touchPosition)));
                
            foreach (RaycastHit hit in hits)
            {
                //Debug.Log("ctig10 WE PRESSED: " + hit.transform.name);
                //Debug.Log("ctig10 RMH: " + roadMeshHolder);
                //Debug.Log("ctig10 RMH T: " + roadMeshHolder.transform);
                //Debug.Log("ctig10 RMH N: " + roadMeshHolder.name);
                    
                if (hit.transform.gameObject.Equals(roadMeshHolder))
                {
                    //Debug.Log("ctig10 WE HIT SAMPLE TRACK");
                    carGO = Instantiate(carPrefab, hit.point, Quaternion.identity);
                    placementIdx = 2;
                }
                //else
                //{
                //    Debug.Log("ctig10 one: " + hit.transform.name);
                //    Debug.Log("ctig10 two: " + roadMeshHolder.transform.name);
                //}
            }
        }
    }

    public void placeTrack()
    {
        placementIdx = 1;
        placeButtonGO.SetActive(false);
        onScreenInput.SetActive(true);

        // disable tracking and all current trackables
        planeManager.enabled = false;
        foreach (var trackablePlane in planeManager.trackables)
        {
            trackablePlane.gameObject.SetActive(false);
        }

        // anchor the track
        ARPlane plane = planeManager.GetPlane(currentNearestHit.trackableId);
        ARAnchor point;
        if (plane != null)
        {
            point = anchorManager.AttachAnchor(plane, currentNearestHit.pose);
            Debug.Log("ctig10 Added an anchor to a plane " + currentNearestHit);
        }
        else
        {
            point = anchorManager.AddAnchor(currentNearestHit.pose); //is obsolete but we can ignore this for now
            Debug.Log("Added another anchor " + currentNearestHit);

        }

        track.transform.parent = point.transform;
    }
}
