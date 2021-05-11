using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkCollider : MonoBehaviour
{
    [SerializeField] private int renderDistance;
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

    public void SetRenderDistance(int distance)
    {
        Debug.Log("Changing render distance");
        collider.radius = distance;
        if(distance == collider.radius)
        {
            Debug.LogError("Encountered a problem while setting render distance");
        }
        else
        {
            Debug.Log("Successfully changed render distance");
        }
    }
}
