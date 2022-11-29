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
    Vector3 up = Vector3.up * 0.5f;

    //'Get checkpoint prefab

    // Start is called before the first frame update
    void Start()
    {
        Generate();
    }

    // public GameObject checkPoint;
    public GameObject holder;
    public float spacing = 3;

    const float minSpacing = .1f;

    void Generate()
    {
        if (pathCreator != null/* && prefab != null*/ && holder != null)
        {
            DestroyObjects();

            VertexPath path = pathCreator.path;
            RoadMeshCreator roadMeshCreator = pathCreator.GetComponent<RoadMeshCreator>();

            // Get width from Path Creator gameobject
            width = GetComponent<RoadMeshCreator>().roadWidth;
            
            checkPoint.transform.localScale = new Vector3(width, 3, 0.1f);
            spacing = Mathf.Max(minSpacing, spacing);
            float dst = 0;

            while (dst < path.length)
            {
                Vector3 point = path.GetPointAtDistance(dst);
                Quaternion rot = path.GetRotationAtDistance(dst);
 
                Instantiate(checkPoint, point + up, rot, holder.transform);
                dst += spacing;
            }
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
            Generate();
        }
    }
}
