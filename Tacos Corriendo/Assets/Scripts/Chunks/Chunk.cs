using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Chunk : MonoBehaviour
{
    [HideInInspector]
    public bool isRoad;
    [HideInInspector]
    public int building_ID;
    public int road_ID;
    [HideInInspector]
    public bool isLoaded;
    Camera mainCam;
    Transform CamPosition;
    [HideInInspector]
    public BuildingRepository Buildings;
    GameObject LoadedObject;
    [HideInInspector]
    public int x;
    [HideInInspector]
    public int y;
    [HideInInspector]
    public int n;
    // Start is called before the first frame update
    void Awake()
    {
        Buildings = (BuildingRepository)AssetDatabase.LoadAssetAtPath("Assets/WorldGeneration/Building Repository.asset", typeof(BuildingRepository));
        mainCam = Camera.main;
        CamPosition = mainCam.transform;
        building_ID = Random.RandomRange(0, Buildings.Buildings.Length);
        road_ID = Random.RandomRange(0, Buildings.Roads.Length);
        LoadObject();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadObject()
    {
        if (isRoad)
        {
            LoadedObject = Instantiate(Buildings.Roads[road_ID], new Vector3(-15f + transform.position.x, 0f, 15f + transform.position.z), transform.rotation);
        }
        else if(!isRoad)
        {
            LoadedObject = Instantiate(Buildings.Buildings[building_ID], new Vector3(-15f + transform.position.x, 0f, 15f + transform.position.z), transform.rotation);
        }
        if (isRoad)
        {
            Debug.Log("loading IsRoad object at (x=" + x + ",y=" + y + ",n=" + n + ")");
        }
        isLoaded = true;
        LoadedObject.transform.parent = transform;
    }
    public void UnloadObject()
    {
        if (isRoad)
        {
            Debug.Log("unloading IsRoad object at (x=" + x + ",y=" + y + ",n=" + n + ")");
        }
        Destroy(LoadedObject);
        isLoaded = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "MainCamera")
        {
            LoadObject();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "MainCamera")
        {
            UnloadObject();
        }
    }
}
