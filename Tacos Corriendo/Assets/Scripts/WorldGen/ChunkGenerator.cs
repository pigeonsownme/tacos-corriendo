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
    private string[] linesInFile;
    private int n;
    private int r;
    private int numberofroads;
    private GameObject player;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Truck kun");
        RoadmapGenerator(width, length);
        linesInFile = ReadRoadmap();
        Debug.Log("Generating roads");
        Debug.Log("there are " + linesInFile.Length + " lines");
        for (int i = 0; i < width; i++)
        {
            char[] chara = linesInFile[i].ToCharArray();
            for(int e = 0; e < length; e++)
            {

                Chunks[e * i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Chunks[e * i].transform.position = new Vector3(i * 30, 0, e * 30);
                Chunks[e * i].transform.rotation = Quaternion.Euler(0, 0, 0);
                Chunks[e * i].transform.localScale = new Vector3(30, 30, 30);
                Chunks[e * i].GetComponent<MeshRenderer>().enabled = false;
                Chunks[e * i].GetComponent<BoxCollider>().isTrigger = true;
                Chunks[e * i].transform.parent = transform;
                Chunk chunkComponent = Chunks[e * i].AddComponent<Chunk>();
                chunkComponent.x = e;
                chunkComponent.y = i;
                chunkComponent.n = i * e;
                Chunks[e * i].tag = "ChunkObject";
                if(chara[e].ToString() == "0")
                {
                    numberofroads++;
                    chunkComponent.isRoad = false;
                }
                else if (chara[e].ToString() == "1")
                {
                    numberofroads++;
                    chunkComponent.isRoad = true;
                    chunkComponent.road_ID = 0;
                }
                else if (chara[e].ToString() == "2")
                {
                    numberofroads++;
                    chunkComponent.isRoad = true;
                    chunkComponent.road_ID = 1;
                }
                else if (chara[e].ToString() == "3")
                {
                    numberofroads++;
                    chunkComponent.isRoad = true;
                    chunkComponent.road_ID = 2;
                }
                else if (chara[e].ToString() == "4")
                {
                    numberofroads++;
                    chunkComponent.isRoad = true;
                    chunkComponent.road_ID = 3;
                }
                else if (chara[e].ToString() == "5")
                {
                    numberofroads++;
                    chunkComponent.isRoad = true;
                    chunkComponent.road_ID = 4;
                }
                else if (chara[e].ToString() == "6")
                {
                    numberofroads++;
                    chunkComponent.isRoad = true;
                    chunkComponent.road_ID = 5;
                }
                else if (chara[e].ToString() == "7")
                {
                    numberofroads++;
                    chunkComponent.isRoad = true;
                    chunkComponent.road_ID = 6;
                }
                else if (chara[e].ToString() == "8")
                {
                    numberofroads++;
                    chunkComponent.isRoad = true;
                    chunkComponent.road_ID = 7;
                }
                else if (chara[e].ToString() == "9")
                {
                    numberofroads++;
                    chunkComponent.isRoad = true;
                    chunkComponent.road_ID = 8;
                }
                else if (chara[e].ToString() == "A")
                {
                    numberofroads++;
                    chunkComponent.isRoad = true;
                    chunkComponent.road_ID = 9;
                }
                else if (chara[e].ToString() == "B")
                {
                    numberofroads++;
                    chunkComponent.isRoad = true;
                    chunkComponent.road_ID = 10;
                }
                if(chunkComponent.isRoad && r == 0)
                {
                    r++;
                    player.transform.position = new Vector3(chunkComponent.transform.position.x, player.transform.position.y, chunkComponent.transform.position.z);
                }
                n++;
                
            }
        }
        Debug.Log("Read document: there are " + numberofroads + " roads and " + ((width * length) - numberofroads) + " buildings");
    }

    private string[] ReadRoadmap()
    {
        Debug.Log("Reading Roadmap");
        string path = Application.dataPath + "/Scripts/WorldGen/LevelGenerator/level.txt";
        Debug.Log("found road map at " + path);
        Chunks = new GameObject[width * length];
        StreamReader reader = new StreamReader(path);
        string file = reader.ReadToEnd();
        string[] lines = file.Split('\n');
        Debug.Log("Roadmap has been read");
        reader.Close();
        return lines;
    }
    public void RoadmapGenerator(int width, int length)
    {
        Debug.Log("Generating roadmap");
        string strCmdText;
        strCmdText = @"/C Assets\Scripts\WorldGen\LevelGenerator\main.exe " + width + " " + length + " 30 35 5";
        Debug.Log("opening cmd with args " + strCmdText);
        try
        {
            System.Diagnostics.Process.Start("CMD.exe", strCmdText);
            Debug.Log("Loaded road map");
        }
        catch
        {
            Debug.LogError("Could not load road map");
        }
        
    }
}
