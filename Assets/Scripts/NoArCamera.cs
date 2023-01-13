using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoArCamera : MonoBehaviour
{
    //[SerializeField]
    private GameObject toFollow;
    
    // Start is called before the first frame update
    void Start()
    {
        toFollow = GameObject.Find("Car(Clone)");
    }

    // Update is called once per frame
    void Update()
    {
        if (toFollow == null) toFollow = GameObject.Find("Car(Clone)");
        if (toFollow == null) return;
            
        transform.LookAt(toFollow.transform);
    }
}
