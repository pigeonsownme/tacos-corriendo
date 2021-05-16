using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class People : MonoBehaviour
{
    [Range(0, 6)]
    [SerializeField] int MaxSize;
    [HideInInspector]
    public int GroupSize;
    // Start is called before the first frame update
    void Start()
    {
        GroupSize = Random.RandomRange(0, MaxSize);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
