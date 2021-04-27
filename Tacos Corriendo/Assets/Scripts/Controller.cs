using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    //Cases
    [Header("Cases")]
    public bool cutscenes = false;
    public bool playing = true;
    [Header("Status")]
    public bool canMove;
    public bool canInteract;
    //Player vars
    [Header("Movement")]
    [SerializeField] public float speed;
    //Controls
    [Header("Controls")]
    public KeyCode forward;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
