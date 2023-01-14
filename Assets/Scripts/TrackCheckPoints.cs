using PathCreation;
using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class TrackCheckPoints : PathSceneTool
{ 
    public CheckPointSingle checkPoint;
    float width;
    Vector3 up = Vector3.up * 0.5f;

    // Lowerbound Barrier
    public GameObject lowerBounds;


    ///////////////////////////////////////////////////////////////
    /// Checkpoints are created at the start of the game
    /// And updated when the game starts, not when the path changes.
    ///////////////////////////////////////////////////////////////

    public GameObject holder;
    public float spacing = 3;
    const float minSpacing = .1f;

    void Generate()
    {

        if (pathCreator != null/* && prefab != null*/ && holder != null)
        {
            DestroyObjects();
            
            // path = pathCreator.path;
            //RoadMeshCreator roadMeshCreator = pathCreator.GetComponent<RoadMeshCreator>();

            // Get width from Path Creator gameobject
            width = GetComponent<RoadMeshCreator>().roadWidth * 2;

            // Change the checkpoint size. (height, width, thickness)
            checkPoint.transform.localScale = new Vector3(0.01f, width, 0.1f); // This changes the prefab  directly cause we dont instanciate it

            spacing = Mathf.Max(minSpacing, spacing);
            
            float dst = spacing;
            int i = 0;

            CheckPointSingle checkPointFirst;
            CheckPointSingle checkPointLast;
            CheckPointSingle c;
            checkPoint.name = "CheckPoint " + i; 

            Vector3 point = pathCreator.path.GetPointAtDistance(dst);
            Quaternion rot = pathCreator.path.GetRotationAtDistance(dst);
            
            //checkPointLast = Instantiate(checkPoint, point + up, rot, holder.transform);
            checkPointLast = Instantiate(checkPoint,
                new Vector3(
                    point.x,
                    point.y + checkPoint.GetComponent<BoxCollider>().size.y * checkPoint.transform.localScale.y / 32,
                    point.z),
                rot, holder.transform);
            checkPointLast.gameObject.GetComponent<MeshRenderer>().enabled = false;

            checkPointFirst = checkPointLast;
            dst += spacing;
            i += 1;
            while (dst < pathCreator.path.length)
            {
                point = pathCreator.path.GetPointAtDistance(dst);
                rot = pathCreator.path.GetRotationAtDistance(dst);

                //Debug.Log("CheckPoint name is " + checkPoint.name);
                checkPoint.name = "CheckPoint " + i;
                c = Instantiate(checkPoint,
               new Vector3(
                    point.x,
                    point.y + checkPoint.GetComponent<BoxCollider>().size.y * checkPoint.transform.localScale.y / 32,
                    point.z),
                rot, holder.transform);
                checkPointLast.NextCheckpoint = c;
                checkPointLast = c;

                if (i != 1)
                    checkPointLast.gameObject.GetComponent<MeshRenderer>().enabled = false;

                dst += spacing;
                i += 1;
            }
            checkPointLast.NextCheckpoint = checkPointFirst;

        }

    }

    void lowerBoundsParameters()
    {
        // if lowerBounds GameObject exists, destroy it

        float lowestPoint = 0;
        float maxX = 0;
        float minX = 0;
        float maxZ = 0;
        float minZ = 0;

        for (int i = 0; i < pathCreator.path.NumPoints; i++)
        {
            if (pathCreator.path.GetPoint(i).y < lowestPoint)
            {
                lowestPoint = pathCreator.path.GetPoint(i).y;
            }
            if (pathCreator.path.GetPoint(i).x > maxX)
            {
                maxX = pathCreator.path.GetPoint(i).x;
            }
            if (pathCreator.path.GetPoint(i).x < minX)
            {
                minX = pathCreator.path.GetPoint(i).x;
            }
            if (pathCreator.path.GetPoint(i).z > maxZ)
            {
                maxZ = pathCreator.path.GetPoint(i).z;
            }
            if (pathCreator.path.GetPoint(i).z < minZ)
            {
                minZ = pathCreator.path.GetPoint(i).z;
            }
        }

        // get a buffer of half x and z
        float bufferX = (maxX - minX) / 3;
        float bufferZ = (maxZ - minZ) / 3;
        minX -= bufferX;
        maxX += bufferX;
        minZ -= bufferZ;
        maxZ += bufferZ;

        // set the position of the lower bounds to the lowest point
        lowerBounds.transform.position = new Vector3(0, lowestPoint - 1, 0);

        // set the scale of the lower bounds to the max and min x and z
        lowerBounds.transform.localScale = new Vector3(maxX - minX, 1, maxZ - minZ);
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
            lowerBoundsParameters();
        }
    }

    void Start()
    {
        // Count the number of GameObjects with the tag "Car" and create an array of that size
        // carProgress = new int[GameObject.FindGameObjectsWithTag("Car").Length];
        if (pathCreator != null)
        {
            Generate();
        }
    }
    
}
