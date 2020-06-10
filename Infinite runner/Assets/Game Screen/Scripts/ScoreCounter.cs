using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreCounter : MonoBehaviour
{
    // Start is called before the first frame update
    public static float scoreCounter = 0;
    public float startingTime=1f;
    public TextMeshProUGUI text;
    public static bool tick;
    public Transform playerRotation; 
    void Start()
    {
        tick = true;
        startingTime = Spawner.levelDistance / Spawner.levelSpeed;
        text.text = scoreCounter.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!Player.notMoving&&playerRotation.eulerAngles.y>=180f&&playerRotation.eulerAngles.y < 360f&&tick)
            scoreCounter += Time.deltaTime;
        text.text = Mathf.Round(scoreCounter / startingTime*200).ToString();
            
    }

    public static void resetScore()
    {
        scoreCounter = 0f;
    }
    public static void addExtraScore()
    {
        scoreCounter += 10;
    }
}
