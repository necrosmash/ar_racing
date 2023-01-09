using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerBounds : PathSceneTool
{
    public GameObject lowerBounds;
    private CheckPointSingle currentCheckpoint;

    // on collision with the lower bounds, reset the car to the current checkpoint
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Car has hit the lower bounds");
        if (other.gameObject.tag == "Car")
        {
            // get the car's current checkpoint
            currentCheckpoint = other.gameObject.GetComponent<SimpleCarController>().currentCheckpoint;

            // position the car at the current checkpoint
            other.gameObject.transform.position = currentCheckpoint.transform.position;

            // rotate the car to face the direction of the track
            other.gameObject.transform.rotation = Quaternion.LookRotation(pathCreator.path.GetDirection(0));

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

    protected override void PathUpdated()
    {
        if (pathCreator != null)
        {
            lowerBoundsParameters();
        }
    }
}
