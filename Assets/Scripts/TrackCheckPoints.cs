using PathCreation;
using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckPoints : PathSceneTool
{
    public GameObject checkPoint;
    GameObject path;
    
    float width;
    public float checkpointWidth;

    Vector3 up = Vector3.up * 0.5f;
    ///////////////////////////////////////////////////////////////
    /// Checkpoints are created at the start of the game
    /// And updated when the game starts, not when the path changes.

   /* void Start()
    {
        Transform checkpointsTransform = transform.Find("Checkpoints");

        foreach (Transform checkpointSingleTransform in checkpointsTransform)
        {
            Debug.Log(checkpointSingleTransform);
        }
    }*/

    // public GameObject checkPoint;
    public GameObject holder;
    public float spacing = 3;

    const float minSpacing = .1f;
    
    void Generate()
    {
        Debug.Log("up: " + up);

        if (pathCreator != null/* && prefab != null*/ && holder != null)
        {
            DestroyObjects();

            VertexPath path = pathCreator.path;
            RoadMeshCreator roadMeshCreator = pathCreator.GetComponent<RoadMeshCreator>();

            // Get width from Path Creator gameobject
            width = GetComponent<RoadMeshCreator>().roadWidth * 2;
            Debug.Log("Width: " + width);
            // Change the checkpoint size. (height, width, thickness)
            checkPoint.transform.localScale = new Vector3(1, width, 0.1f);
            
            spacing = Mathf.Max(minSpacing, spacing);
            float dst = 0;

            while (dst < path.length)
            {
                Vector3 point = path.GetPointAtDistance(dst);
                Quaternion rot = path.GetRotationAtDistance(dst);
 
                Instantiate(checkPoint, point + up, rot, holder.transform);
                dst += spacing;
            }

            /*Transform checkpointsTransform = transform.Find("Checkpoints");

            foreach (Transform checkpointSingleTransform in checkpointsTransform)
            {
                Debug.Log(checkpointSingleTransform);
            }*/
        }
    }

    void DestroyObjects()
    {
        int numChildren = holder.transform.childCount;
        for (int i = numChildren - 1; i >= 0; i--)
        {
            DestroyImmediate(holder.transform.GetChild(i).gameObject, false);
        }
    }

    protected override void PathUpdated()
    {
        if (pathCreator != null)
        {
            // width = GetComponent<RoadMeshCreator>().roadWidth * 2;
            Generate();
        }
    }
}
