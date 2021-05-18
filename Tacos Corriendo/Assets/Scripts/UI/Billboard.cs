using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Transform truckKun;
    [SerializeField] bool orientation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (orientation)
        {
            transform.LookAt(target, Vector3.up);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, truckKun.rotation.y, 0);
        }
    }
}
