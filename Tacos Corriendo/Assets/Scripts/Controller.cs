using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    //Cases
    [Header("Cases")]
    public bool cutscenes = false;
    public bool playing = true;
    //Status
    [Header("Status")]
    public bool canMove;
    public bool canInteract;
    //Player vars
    [Header("Movement")]
    [SerializeField] public float motorForce;
    [SerializeField] public float breakForce;
    //Controls
    [Header("Controls")]
    public KeyCode forward = KeyCode.W;
    public KeyCode backwards = KeyCode.S;
    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;
    public KeyCode breaking = KeyCode.Space;
    [SerializeField] float MaxSteeringAngle = 75f;
    private bool forwPress;
    private bool backpress;
    private bool leftPress;
    private bool rightPress;
    //Current physics state
    [Header("Current movement")]
    public bool isMoving;
    public bool isBraking;
    public float CurrentBrakeForce;
    public float CurrentSteerAngle;
    private float SteerAngle;
    [SerializeField]
    private float SteerTimer = 0;
    //Physics
    [Header("Physics")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float mass;
    [Header("Wheel colliders")]
    [SerializeField] private WheelCollider FrontLeftWheelCollider;
    [SerializeField] private WheelCollider FrontRightWheelCollider;
    [SerializeField] private WheelCollider BackLeftWheelCollider;
    [SerializeField] private WheelCollider BackRightWheelCollider;
    public Vector3 rotation;
    public Vector3 LocalRot;
    //Camera
    [Header("Camera Settings")]
    [SerializeField] private GameObject cameraAnchor;
    [SerializeField] public Camera MainCamera;
    [SerializeField] public float CamMovespeed;
    private Transform cameraAnchorTransform;

    //System
    private float Timer = 180;

    // Start is called before the first frame update
    void Start()
    {
        //References check
        if(rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
        rb.mass = mass;
        rb.centerOfMass -= new Vector3(0f, 0.9f, 0f);
        cameraAnchorTransform = cameraAnchor.transform;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        CameraFollow();
        LocalRot = transform.localRotation.eulerAngles;
        rotation = transform.rotation.eulerAngles;
        if (Input.GetKey(KeyCode.T))
        {
            transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
        }
    }

    private void GetInput()
    {
        forwPress = Input.GetKey(forward);
        leftPress = Input.GetKey(left);
        rightPress = Input.GetKey(right);
        isBraking = Input.GetKey(breaking);
        backpress = Input.GetKey(backwards);
        if(!rightPress || !leftPress)
        {
            if (rightPress && !leftPress)
            {
                SteerTimer += Time.deltaTime;
            }
            else if(leftPress && !rightPress)
            {
                SteerTimer -= Time.deltaTime;
            }
            else
            {
                SteerTimer = 0;
            }

            SteerTimer = Mathf.Clamp(SteerTimer, -1f, 1f);
        }
        else
        {
            SteerTimer = 0;
        }
        

    }

    private void HandleMotor()
    {
        if (forwPress)
        {
            FrontLeftWheelCollider.motorTorque = motorForce;
            FrontRightWheelCollider.motorTorque = motorForce;
        }
        else
        {
            FrontLeftWheelCollider.motorTorque = 0;
            FrontRightWheelCollider.motorTorque = 0;
        }
        if (isBraking)
        {
            CurrentBrakeForce = breakForce;
        }
        else
        {
            CurrentBrakeForce = 0f;
        }
        if (backpress)
        {
            FrontLeftWheelCollider.motorTorque = -motorForce/2;
            FrontRightWheelCollider.motorTorque = -motorForce/2;
        }
        ApplyBreaking();

    }

    private void ApplyBreaking()
    {
        FrontLeftWheelCollider.brakeTorque = CurrentBrakeForce;
        FrontRightWheelCollider.brakeTorque = CurrentBrakeForce;
        BackLeftWheelCollider.brakeTorque = CurrentBrakeForce;
        BackRightWheelCollider.brakeTorque = CurrentBrakeForce;
    }
    
    private void HandleSteering()
    {
        CurrentSteerAngle = MaxSteeringAngle * SteerTimer;
        FrontLeftWheelCollider.steerAngle = CurrentSteerAngle;
        FrontRightWheelCollider.steerAngle = CurrentSteerAngle;
    }

    private void CameraFollow()
    {
        var targetRotation = transform.rotation;
        
        cameraAnchorTransform.rotation = Quaternion.Slerp(cameraAnchorTransform.rotation, targetRotation, (CamMovespeed) * Time.deltaTime);
        cameraAnchorTransform.position = transform.position;
    }
}
