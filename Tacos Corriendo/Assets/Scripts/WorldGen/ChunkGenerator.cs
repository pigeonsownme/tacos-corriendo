using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    [SerializeField] int width;
    [SerializeField] int length;
    [SerializeField] int minForks;
    [SerializeField] int maxForks;
    public GameObject[] Chunks;
    // Start is called before the first frame update
    void Start()
    {
        var forks = new List<Vector2>();
        int nForks = Random.RandomRange(minForks, maxForks);
        
        for (int i = 0; i < nForks; i++)
        {
            int x = Random.RandomRange(0, width);
            int y = Random.RandomRange(0, length);
            forks.Add(new Vector2(x, y));
        }

        Chunks = new GameObject[width*length];
        for (int i = 0; i < width; i++)
        {
            for(int e = 0; e < length; e++)
            {
                Debug.Log($"Generating cube ({i},{e}) at coords ({i*10};{e*10})");
                Chunks[e * i] = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), new Vector3(i*30, 0, e*30), Quaternion.Euler(0,0,0), transform);
                Chunks[e * i].transform.localScale = new Vector3(1, 30, 1);
                Chunks[e * i].GetComponent<MeshRenderer>().enabled = false;
                Chunks[e * i].GetComponent<BoxCollider>().isTrigger = true;
                Chunks[e * i].AddComponent<Chunk>();
                Chunks[e * i].tag = "ChunkObject";

                if (forks.Contains(new Vector2(i, e))) Chunks[e * i].GetComponent<Chunk>().isRoad = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
