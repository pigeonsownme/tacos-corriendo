using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Chunk : MonoBehaviour
{
    public bool isRoad;
    [HideInInspector]
    public int building_ID;
    public int road_ID = 0;
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
    void Start()
    {

        Buildings = Resources.Load<BuildingRepository>("BuildingRep/Building Repository");
        mainCam = Camera.main;
        CamPosition = mainCam.transform;
        if (!isRoad)
        {
            building_ID = Random.Range(0, Buildings.Buildings.Length);
        }
        if (isRoad)
        {
            LoadRoad();
            LoadedObject.transform.parent = this.transform;
        }
        else
        {
            LoadBuilding();
        }
        UnloadObject();
        
    }
    public void LoadBuilding()
    {
        if (isRoad)
        {
            Debug.Log("loading building on a road??");
        }
        LoadedObject = Instantiate(Buildings.Buildings[building_ID], new Vector3(-15f + transform.position.x, 0f, 15f + transform.position.z), transform.rotation);
        isLoaded = true;
        LoadedObject.transform.parent = transform;
        isRoad = false;
    }
    public void LoadRoad()
    {
        LoadedObject = Instantiate(Buildings.Roads[road_ID], new Vector3(-15f + transform.position.x, 0f, 15f + transform.position.z), transform.rotation);
        isLoaded = true;
        LoadedObject.transform.parent = this.transform;
        isRoad = true;
    }
    public void UnloadObject()
    {
        if (isRoad)
        {
        }
        Destroy(LoadedObject);
        isLoaded = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "MainCamera")
        {
            if (isRoad)
            {
                LoadRoad();
            }
            else
            {
                LoadBuilding();
            }
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
