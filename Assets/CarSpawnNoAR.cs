using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawnNoAR : MonoBehaviour
{
    [SerializeField]
    private GameObject carPrefab;
    private GameObject carGO;

    [SerializeField]
    private GameObject track;

    // Start is called before the first frame update
    void Start()
    {
        carGO = Instantiate(
            carPrefab,
            new Vector3(
                track.transform.Find("CheckPointHolder").transform.Find("CheckPoint 0(Clone)").transform.position.x,
                track.transform.Find("CheckPointHolder").transform.Find("CheckPoint 0(Clone)").transform.position.y + 0.1f,
                track.transform.Find("CheckPointHolder").transform.Find("CheckPoint 0(Clone)").transform.position.z
            ),
            Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
