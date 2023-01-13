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
    
    private bool isPlacing; // whether or not we're currently placing a track
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
        isPlacing = true;
    }

    void Update()
    {
        Debug.Log("ctig11 y difference: " +
            (track.transform.Find("Path Creator").transform.position.y - track.transform.Find("CheckPointHolder").transform.position.y).ToString());

        if (!isPlacing) return;
        if (raycastManager == null || track == null) return;
        
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var arRaycastHits = new List<ARRaycastHit>();
        bool isHit = raycastManager.Raycast(screenCenter, arRaycastHits, TrackableType.Planes);

        if (isHit && isPlacing && arRaycastHits.Count > 0)
        {
            currentNearestHit = arRaycastHits[0];
            track.transform.SetPositionAndRotation(currentNearestHit.pose.position, currentNearestHit.pose.rotation);
            Debug.Log("ctig10 setting track to active");

            roadMesh.enabled = true;
        }
    }

    public void placeTrack()
    {
        isPlacing = false;
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

        GameObject startingPoint = track.transform.Find("CheckPointHolder").transform.Find("CheckPoint 0(Clone)").gameObject;
        carGO = Instantiate(
            carPrefab,
            new Vector3(
                startingPoint.transform.position.x,
                startingPoint.transform.position.y + 0.1f,
                startingPoint.transform.position.z
            ),
            Quaternion.identity);
    }
}
