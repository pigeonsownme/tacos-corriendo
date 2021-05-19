using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Transform truckKun;
    // Start is called before the first frame update
    void Start()
    {
        truckKun = GameObject.Find("Truck kun").transform;
        target = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target, Vector3.up);
    }
}
