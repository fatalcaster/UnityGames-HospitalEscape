using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallsMovement : MonoBehaviour
{
    #region Public Properties

    public static float speed;
    public float normalSpeed = 5f;
    public float slowDown = 2f;
    public static float verticalSpeedPercentage;
    static public bool collidingForward = false;
    static public bool collidingBackward = false;
    static public bool objectStill = true;

    #endregion


    #region Start&Update
    void Start()
    {
        speed = normalSpeed;  
    }
    void Update()
    {
        if (Player.health == 0) return;
        movementController();
        destroyChecker();
    }
    #endregion

    /// <summary>
    /// Reads input and corresponds to that
    /// </summary>
    private void movementController()
    {
        if (Input.GetKey(KeyCode.S) && !collidingBackward)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed 
                                *verticalSpeedPercentage, Space.Self); //LEFT
            if (objectStill) objectStill = false;
        }
        else if (Input.GetKey(KeyCode.W) && !collidingForward)
        {
            transform.Translate(Vector3.right * Time.deltaTime 
                                *verticalSpeedPercentage* speed, Space.Self); //RIGHT
            if (objectStill) objectStill = false;
        }
        else if (!objectStill)
            objectStill = true;
    }

    /// <summary>
    /// Sets object conditions to moveable
    /// </summary>
    void setObjectToMove()
    {
        collidingForward = false;
        collidingBackward = false;
        objectStill = false;
    }

    /// <summary>
    /// Checks if the object is out of range
    /// </summary>
    private void destroyChecker()
    {
        if (transform.position.x > 15 || transform.position.y < -5f)
            Destroy(gameObject);
    }

    #region Collision
    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
            playerCollision(collision);


    }
    
    private void OnCollisionExit(Collision collision)
    {
        setObjectToMove();
    }
    protected void playerCollision(Collision collision)
    {
        Vector3 playerPosition = collision.gameObject.transform.position;
        Vector3 objectPosition = gameObject.transform.position;
        float objectLength = gameObject.transform.localScale.x;
        if (playerPosition.x >= objectPosition.x + objectLength / 2f)
        {
            collidingForward = true;
            objectStill = true;
            Debug.Log("Colliding Forward");
        }
        if (playerPosition.x <= objectPosition.x - objectLength / 2f)
        {
            collidingBackward = true;
            objectStill = true;
            Debug.Log("Colliding Backward");
        }
        speed = slowDown;
    }

    #endregion

}
