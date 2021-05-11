using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkCollider : MonoBehaviour
{
    [SerializeField] private float renderDistance;
    private SphereCollider collider;
    // Start is called before the first frame update
    void Start()
    {
        collider = gameObject.AddComponent<SphereCollider>();
        collider.isTrigger = true;
        SetRenderDistance(renderDistance);
    }

    private void Update()
    {
        if (renderDistance != collider.radius)
        {
            SetRenderDistance(renderDistance);
        }
    }

    public void SetRenderDistance(float distance)
    {
        Debug.Log("Changing render distance");
        collider.radius = distance;
        float newRad = collider.radius;
        if(distance == newRad)
        {
            Debug.LogError("Encountered a problem while setting render distance (new render distance is " + distance + " while collider radius is "+ newRad);
        }
        else
        {
            Debug.Log("Successfully changed render distance");
        }
    }
}
