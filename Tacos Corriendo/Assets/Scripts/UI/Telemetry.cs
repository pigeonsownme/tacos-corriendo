using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Telemetry : MonoBehaviour
{
    public float timeLeft;
    public float speed;
    public int money;
    [SerializeField] Text timer;
    [SerializeField] Text Speedo;
    [SerializeField] Text Capital;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTelemetry();
    }
    private void UpdateTelemetry()
    {
        float minutes = Mathf.Floor(timeLeft/60);
        float seconds = Mathf.Floor(timeLeft - (60 * minutes));
        timer.text = minutes + ":" + seconds;
        Speedo.text = speed.ToString("F1");
        Capital.text = money.ToString();
    }
}
