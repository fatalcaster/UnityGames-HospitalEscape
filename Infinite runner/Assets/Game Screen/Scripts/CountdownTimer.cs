using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class CountdownTimer : MonoBehaviour
{
    // Start is called before the first frame update

    //Reference to the TimerUI part
    public TextMeshProUGUI timer;
    public static float realTimer = 100f;

    //Timer on start
    void Start()
    {
        //textBox = FindObjectOfType<TextMeshProUGUI>();
        timer.text = (Spawner.levelTime*1.2f).ToString();
        resetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (realTimer > 0f && Player.alive) realTimer -= Time.deltaTime;
        else
        {
            Player.health = 0;
            ScoreCounter.tick = false;
        }
        timer.text = Math.Round(realTimer, 1).ToString();
    }
    public static void resetTimer()
    {
        realTimer = 1.4f * Spawner.levelDistance / Spawner.levelSpeed;
    }
    public static void addExtraTime()
    {
        Debug.Log("Pozvan sam");
        realTimer += 5f;
    }
}
