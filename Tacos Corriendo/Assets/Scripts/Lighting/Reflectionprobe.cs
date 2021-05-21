using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflectionprobe : MonoBehaviour
{
    Transform mainCam;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(mainCam.position, Vector3.down, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position,Vector3.down * hit.distance, Color.yellow);
            transform.position = hit.point;
        }
    }
}
