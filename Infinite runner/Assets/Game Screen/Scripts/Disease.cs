using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disease : MonoBehaviour
{
    /// <summary>
    /// If the main timer ticks disease should kill the player - still not implemented 
    /// </summary>
    private float speed = 1f;
    private float startingPositionX = 15f;


    // Update is called once per frame
    void Update()
    {

        if(WallsMovement.objectStill)
            transform.Translate(Vector3.left * Time.deltaTime * speed, Space.Self);
        if(!WallsMovement.objectStill && transform.position.x< startingPositionX)
            transform.Translate(Vector3.right * Time.deltaTime * speed, Space.Self);

    }
}
