using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CountdownTimer : MonoBehaviour
{
    // Start is called before the first frame update

    //Reference to the TimerUI part
    public TextMeshProUGUI timer;

    //Timer on start
    public float timeStart = 5f;
    void Start()
    {
        //textBox = FindObjectOfType<TextMeshProUGUI>();
        timer.text = timeStart.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(timeStart>0f) timeStart -= Time.deltaTime;
        timer.text = Math.Round(timeStart, 1).ToString();
    }
}
