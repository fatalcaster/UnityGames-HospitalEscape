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

    static bool spreading = false;
    public Transform playerRotation;
    // Update is called once per frame
    void Update()
    {
        checkSpreading();
        if(spreading)
            transform.Translate(Vector3.left * Time.deltaTime * speed, Space.Self);
        else if(transform.position.x<15f)
            transform.Translate(Vector3.right * Time.deltaTime * speed, Space.Self);
    }

    void checkSpreading()
    {
        if (WallsMovement.objectStill) return;
        if (Input.GetKey(KeyCode.S) && playerRotation.eulerAngles.y > 180f && playerRotation.eulerAngles.y < 360f)
            spread();
        else if (Input.GetKey(KeyCode.W) && playerRotation.eulerAngles.y < 180f && playerRotation.eulerAngles.y > 0f)
            spread();
        else spreading = false;

    }
    public static void spread()
    {
        spreading = true;
    }
}
