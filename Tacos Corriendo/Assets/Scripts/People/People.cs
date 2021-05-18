using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class People : MonoBehaviour
{
    [Range(0, 6)]
    [SerializeField] int MaxSize;
    public int GroupSize;
    bool road = false;
    public bool hasSpawned;
    GameObject TACOs;
    GameObject spawned;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void pplnshit()
    {
        if (!hasSpawned)
        {
            GroupSize = Random.RandomRange(0, MaxSize);
            TACOs = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Graphics/Prefab/UI/Taco.prefab", typeof(GameObject));
            spawned = Instantiate(TACOs, new Vector3(0.5f, 2, -0.5f), Quaternion.Euler(0,0,0));
            spawned.transform.parent = transform;
            Destroy(GetComponent<Rigidbody>());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Road")
        {
            road = true;
            pplnshit();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (!hasSpawned)
        {
            if (other.tag == "Road")
            {
                road = true;
                pplnshit();
            }
        }
    }
}
