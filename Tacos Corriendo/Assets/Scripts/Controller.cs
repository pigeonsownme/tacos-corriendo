using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    //Cases
    [Header("Cases")]
    public bool cutscenes = false;
    public bool playing = true;
    //Status
    [Header("Status")]
    public bool isfinished = false;
    public bool canMove = false;
    public bool canInteract = false;
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
    private float speed;
    [HideInInspector]
    public int money = 0;
    private bool HasTimeStarted = false;
    private float StartTime = 5;
    private Telemetry telemetryComp;
    private GameObject Starting;
    private Text StartingTimer;
    private float police;
    private float PoliceComing;
    GameObject PoliceBar;
    GameObject ComingBar;
    GameObject policeVignette;
    Image Vig;
    private bool policeCalled = false;

    //Debug
    String log;
    private StreamWriter _writer;
    // Start is called before the first frame update
    void Start()
    {
        _writer = File.AppendText(Application.dataPath + "/logs/log.txt");
        _writer.Write("\n\n=============== Game started ================\n\n");
        DontDestroyOnLoad(gameObject);
        Application.RegisterLogCallback(HandleLog);
        //References check
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
        telemetryComp = GameObject.Find("UI").GetComponent<Telemetry>();
        rb.mass = mass;
        rb.centerOfMass -= new Vector3(0f, 0.9f, 0f);
        cameraAnchorTransform = cameraAnchor.transform;
        StartingTimer = GameObject.Find("Starting In").GetComponent<Text>();
        Starting = GameObject.Find("Image (1)");
        PoliceBar = GameObject.Find("Progress");
        ComingBar = GameObject.Find("ProgressComing");
        policeVignette = GameObject.Find("PoliceVignette");
        Vig = policeVignette.GetComponent<Image>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        
        CalculateTelemetry();
        if (!HasTimeStarted)
        {
            StartGame();
        }
        if (canMove)
        {
            GetInput();
            HandleMotor();
            HandleSteering();
        }
        PoliceHandler();
        CameraFollow();
        LocalRot = transform.localRotation.eulerAngles;
        rotation = transform.rotation.eulerAngles;
        if (Input.GetKey(KeyCode.T))
        {
            transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
        }
    }

    private void HandleLog(string condition, string stackTrace, LogType type)
    {
        var logEntry = string.Format("\n {0} {1} \n {2}\n {3}"
            , DateTime.Now, type, condition, stackTrace);
        _writer.Write(logEntry);
    }
    private void CalculateTelemetry()
    {
        speed = rb.velocity.magnitude;
        telemetryComp.speed = speed * 1.7f;
        
    }
    private void PoliceHandler()
    {
        if(!policeCalled && HasTimeStarted)
        {
            if ((speed * 1.7f) <= 100)
            {
                police += 0.025f;
                police = Mathf.Clamp(police, 0, 85.2f);
            }
            else
            {
                police -= 0.04f + (speed * 1.7f) / 1500;
                police = Mathf.Clamp(police, 0, 85.2f);
            }
            if(police == 85.2f)
            {
                policeCalled = true;
            }
        }
        if(policeCalled && HasTimeStarted)
        {
            if ((speed * 1.7f) <= 100)
            {
                PoliceComing += 0.03f;
                PoliceComing = Mathf.Clamp(PoliceComing, 0, 85.2f);
            }
            else
            {
                PoliceComing -= 0.05f + (speed * 1.7f) / 1500;
                PoliceComing = Mathf.Clamp(PoliceComing, 0, 85.2f);
            }
            if(PoliceComing == 0)
            {
                policeCalled = false;
            }
        }
        
        PoliceBar.transform.localScale = new Vector3(police, 1.0375f, 1);
        Vig.color =new Vector4(Vig.color.r , Vig.color.g, Vig.color.b, PoliceComing / 85.2f);
        ComingBar.transform.localScale = new Vector3(PoliceComing, 1.0375f, 1);
    }
    private void StartGame()
    {
        StartTime -= Time.deltaTime;
        StartingTimer.text = "Starting in \n" + Mathf.Floor(StartTime);
        if (StartTime <= 0)
        {
            Debug.Log("Game is officially starting");
            HasTimeStarted = true;
            canMove = true;
            canInteract = true;
            Destroy(Starting);
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
