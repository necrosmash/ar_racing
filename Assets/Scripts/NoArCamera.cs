using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoArCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject toFollow;
    
    // Start is called before the first frame update
    void Start()
    {
        if (toFollow == null)
        {
            //toFollow = GameObject.Find("Car");
            Debug.LogError("ctig4 to follow is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(toFollow.transform);
    }
}
