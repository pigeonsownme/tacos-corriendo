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
        string path = Application.dataPath + "/Scripts/Chunks/Level.txt";
        string[] lines = File.ReadAllLines(path);
        Debug.Log(File.ReadAllLines(path));
        Chunks = new GameObject[width*length];
        RoadmapGenerator(width / 10, width, length);
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
                char[] characters = lines[i].ToCharArray();
                if(characters[e] != 0)
                {
                    chunkComponent.isRoad = true;
                }
                else
                {
                    chunkComponent.isRoad = false;
                }
                n++;
                chunkComponent.x = e;
                chunkComponent.y = i;
                chunkComponent.n = n;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RoadmapGenerator(int ForkNumberByRow, int MapWidth, int MapLength)
    {
        string path = Application.dataPath + "/Scripts/Chunks/Level.txt";
        int totalCharCount;
        Debug.Log("Generating Roadmap at path " + path + "===============================================================================================================");
        try
        {
            File.Delete(path);
            Debug.Log("Successfuly cleared old level file");
        }
        catch
        {
            Debug.Log("No prior level file found");
        }

        StreamWriter writer = new StreamWriter(path, true);
        string content = "";
        bool isLastRoad = false;
        writer.Write("");
        Debug.Log("Beggining road generation");
        for (int i = 0; i < MapLength; i++)
        {
            for (int e = 0; e < MapWidth; e++)
            {
                if (isLastRoad)
                {
                    content += "0";
                    isLastRoad = false;
                }
                else
                {
                    int gotIt = Random.Range(0, MapWidth);
                    if (gotIt == 1)
                    {
                        content += "1";
                        isLastRoad = true;
                    }
                    else
                    {
                        content += "0";
                    }
                }

            }
            try
            {
                writer.WriteLine(content);
                content = "";
                Debug.Log("Logged road row " + i + "/" + MapLength + ", wrote " + i * MapWidth + " chunks as of now");
            }
            catch
            {
                Debug.LogError("Couldnt write line " + i + "/" + MapLength);
            }
        }
        writer.Close();
        AssetDatabase.ImportAsset(path);
    }
}
