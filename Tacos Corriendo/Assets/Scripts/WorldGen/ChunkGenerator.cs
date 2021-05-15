using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    [SerializeField] int width;
    [SerializeField] int length;
    public GameObject[] Chunks;
    private int n;
    // Start is called before the first frame update
    void Start()
    {
        RoadmapGenerator();
        Chunks = new GameObject[width*length];
       // RoadmapGenerator(width / 10, width, length);
        for (int i = 0; i < width; i++)
        {
            for(int e = 0; e < length; e++)
            {
                Chunks[e * i] = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), new Vector3(i*30, 0, e*30), Quaternion.Euler(0,0,0), transform);
                Chunks[e * i].transform.localScale = new Vector3(1, 30, 1);
                Chunks[e * i].GetComponent<MeshRenderer>().enabled = false;
                Chunks[e * i].GetComponent<BoxCollider>().isTrigger = true;
                Chunk chunkComponent = Chunks[e * i].AddComponent<Chunk>();
                Chunks[e * i].tag = "ChunkObject";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RoadmapGenerator()
    {
        Debug.Log("Generating roadmap");
        string strCmdText;
        strCmdText = @"Assets\Scripts\WorldGen\LevelGenerator\main.exe 50 50 30 35 5";
        Debug.Log("opening cmd with args " + strCmdText);
        System.Diagnostics.Process.Start("CMD.exe", strCmdText);
    }
}
