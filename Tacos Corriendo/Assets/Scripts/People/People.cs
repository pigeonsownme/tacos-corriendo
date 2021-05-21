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
    Transform parent;
    Rigidbody rBody;
    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        parent = GameObject.Find("bubble").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void pplnshit()
    {
        if (!hasSpawned)
        {
            GroupSize = Random.Range(0, MaxSize);
            if(GroupSize > 0)
            {
                TACOs = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Graphics/Prefabs/UI/Taco.prefab", typeof(GameObject));
                spawned = Instantiate(TACOs, new Vector3((transform.position.x + 0.5f * 30), (transform.position.y + 2), (transform.position.z - 0.5f * 30)), Quaternion.Euler(0, 0, 0), parent);
                spawned.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
                rBody.detectCollisions = false;
                Destroy(rBody);
                hasSpawned = true;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Road")
        {
            road = true;
        }
        if(!hasSpawned && road && other.tag == "Tacolider")
        {
            pplnshit();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (hasSpawned && road && other.tag == "Tacolider")
        {
            Destroy(spawned);
        }
    }
}
