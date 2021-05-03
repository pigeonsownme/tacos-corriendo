using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Chunk : MonoBehaviour
{
    [HideInInspector]
    public bool isRoad = false;
    [HideInInspector]
    public int ID;
    [HideInInspector]
    public bool isLoaded;
    Camera mainCam;
    Transform CamPosition;
    [HideInInspector]
    public BuildingRepository Buildings;
    GameObject LoadedObject;
    // Start is called before the first frame update
    void Start()
    {
        Buildings = (BuildingRepository)AssetDatabase.LoadAssetAtPath("Assets/WorldGeneration/Building Repository.asset", typeof(BuildingRepository));
        mainCam = Camera.main;
        CamPosition = mainCam.transform;
        ID = Random.RandomRange(0, Buildings.Buildings.Length);
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
            LoadedObject = Instantiate(Buildings.Roads[ID], new Vector3(-15f,0f,15f), transform.rotation);
        }
        else
        {
            LoadedObject = Instantiate(Buildings.Buildings[ID], new Vector3(-15f + transform.position.x, 0f, 15f + transform.position.z), transform.rotation);
        }
        isLoaded = true;
    }
    public void UnloadObject()
    {
        Destroy(LoadedObject);
        isLoaded = false;
    }
}
