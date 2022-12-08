using PathCreation;
using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class TrackCheckPoints : MonoBehaviour
{ 
    // public CheckPointSingle checkPointClass;
    public CheckPointSingle checkPoint;
    float width;
    Vector3 up = Vector3.up * 0.5f;
    ///////////////////////////////////////////////////////////////
    /// Checkpoints are created at the start of the game
    /// And updated when the game starts, not when the path changes.
    ///////////////////////////////////////////////////////////////

    public GameObject holder;
    public float spacing = 3;

    const float minSpacing = .1f;
    
    public void Generate()
    {
        //if (pathCreator != null/* && prefab != null*/ && holder != null)
        if (holder == null)
        {
            Debug.LogError("no checkpoint holder GO passed to TrackCheckPoint.cs");
            return;
        }

        DestroyObjects();
            
        RoadMeshCreator roadMeshCreator = this.GetComponent<RoadMeshCreator>();

        // Get width from Path Creator gameobject
        width = GetComponent<RoadMeshCreator>().roadWidth * 2;
            
        // Change the checkpoint size. (height, width, thickness)
        checkPoint.transform.localScale = new Vector3(1, width, 0.1f); // This changes the prefab  directly cause we dont instanciate it
            
        spacing = Mathf.Max(minSpacing, spacing);
            
        float dst = spacing;
        int i = 0;

        CheckPointSingle checkPointFirst;
        CheckPointSingle checkPointLast;
        checkPoint.name = "CheckPoint " + i; 

        Vector3 point = roadMeshCreator.Vp.GetPointAtDistance(dst);
        Quaternion rot = roadMeshCreator.Vp.GetRotationAtDistance(dst);
            
        checkPointLast = Instantiate(checkPoint, point + up, rot, holder.transform);
            
        checkPointFirst = checkPointLast;
        dst += spacing;
        i += 1;
        while (dst < roadMeshCreator.Vp.length)
        {
            point = roadMeshCreator.Vp.GetPointAtDistance(dst);
            rot = roadMeshCreator.Vp.GetRotationAtDistance(dst);

            //Debug.Log("CheckPoint name is " + checkPoint.name);
            checkPoint.name = "CheckPoint " + i;
            CheckPointSingle c = Instantiate(checkPoint, point + up, rot, holder.transform);
            checkPointLast.NextCheckpoint = c;
            checkPointLast = c;

            dst += spacing;
            i += 1;
        }
        checkPointLast.NextCheckpoint = checkPointFirst;
    }

    void DestroyObjects()
    {
        int numChildren = holder.transform.childCount;
        for (int i = numChildren - 1; i >= 0; i--)
        {
            DestroyImmediate(holder.transform.GetChild(i).gameObject, false);
        }
    }

    /*public void CarHitCheckpoint(CheckPointSingle checkPointSingle)
    {
        Debug.Log("Car through checkpoint X" + checkPointSingle.checkPointNumber);
        // checkPointSingle.gameObject.GetComponent<SimpleCarController>().lastCheckpoint = gameObject;
    }*/

    
}
