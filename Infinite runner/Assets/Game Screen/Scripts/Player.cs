using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Public Properties

    /// <summary>
    /// Speed variables
    /// </summary>
    
    public float speedNormal=3f;
    public float slowDownSpeed = 1f;
    public static float speed;
    public float rotationSpeed = 200f;


    /// <summary>
    /// Attack properties
    /// </summary>
    public float MaxAttackDelay;
    public float AttackSpeed = 50f;
    public float AttackTime;
    public static bool Attacking = false;

    /// <summary>
    /// Player position and stats
    /// </summary>
    public Vector3 startingPosition;
    public static int health = 3;
    #endregion

    #region Private Members

    static float playersAngle = 0f;
    
    private bool moveableLeft = true;
    private bool moveableRight = true;
    private float horizontalSpeedPercentage = 0f;
    
    float mAttackDelay;
    bool mAttackedRecently = false;

    
    float mAttackTime;

    #endregion

    #region Collision Part

    /// <summary>
    /// Calls every time Players collider hits something
    /// </summary>
    /// <param name="collision"> The object which player countered</param>
    protected void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "spawnedWall" || collision.gameObject.tag == "Wall")
            speed = slowDownSpeed;
        if (collision.gameObject.tag == "Bullet"&&health>0)
            health--;
    }

    /// <summary>
    /// Calls every time Players collider stop hitting something
    /// </summary>
    /// <param name="collision">The object which player countered</param>
    protected void OnCollisionExit(Collision collision)
    {
        speed = speedNormal;
    }
    #endregion

    #region Speed and Movement

    /// <summary>
    /// Receives input and translates the player based on it
    /// </summary>
    void processInput()
    {
        Vector3 temp = transform.forward;
        temp.x = 0f;
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
        else
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(-Vector3.up * rotationSpeed * Time.deltaTime);
        }
        

        //If S is pressed the Player will "go" backwards
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(-temp * Time.deltaTime *
                    slowDownSpeed * horizontalSpeedPercentage, Space.World);
        }
        if (Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.W) && !mAttackedRecently)
        {
            setUpAttack();

        }
        else if (Input.GetKey(KeyCode.W))
            transform.Translate(temp * Time.deltaTime *
                    speed * horizontalSpeedPercentage, Space.World);
    }

    /// <summary>
    /// Configures speed based on the players angle
    /// </summary>
    /*
    private void configureMovementSpeed()
    {
        playersAngle = transform.rotation.eulerAngles.y;
        Debug.Log("players angle" + playersAngle);
        bool negativeMovement = playersAngle > 180f ? false : true;
        playersAngle %= 180f;
        if (playersAngle >= 0f && playersAngle <= 20f)
        {
            horizontalSpeedPercentage = 1f;
            WallsMovement.verticalSpeedPercentage = 0f;
        }
        else if (playersAngle > 20f && playersAngle <= 70f)
        {
            horizontalSpeedPercentage = 0.5f;
            WallsMovement.verticalSpeedPercentage = negativeMovement ? -0.5f : 0.5f;
        }
        else if (playersAngle > 70f && playersAngle < 90f)
        {
            horizontalSpeedPercentage = 0f;
            WallsMovement.verticalSpeedPercentage = negativeMovement ? -1f : 1f;
        }

    }*/
    private void configureMovementSpeed()
    {
        float horizontalSpeedPercentage=0f, verticalSpeedPercentage=0f;
        playersAngle = transform.rotation.eulerAngles.y;
        short angleCase = (short) (Mathf.Floor(playersAngle / 90f) + 1);
        float tempAngle = playersAngle;
        playersAngle %= 90f;
        switch (angleCase)
        {
            case 1:
                horizontalSpeedPercentage = Mathf.Cos(playersAngle)* Mathf.Cos(playersAngle);
                verticalSpeedPercentage  = horizontalSpeedPercentage - 1;
                if (playersAngle == 0f)
                {
                    verticalSpeedPercentage = -1f;
                    horizontalSpeedPercentage = 0f;
                }
                break;
            case 2:
                horizontalSpeedPercentage = Mathf.Sin(playersAngle) * Mathf.Sin(playersAngle);
                verticalSpeedPercentage = horizontalSpeedPercentage - 1;
                if (playersAngle == 0f)
                {
                    verticalSpeedPercentage = 0f;
                    horizontalSpeedPercentage = 1f;
                }
                break;
            case 3:
                horizontalSpeedPercentage = Mathf.Cos(playersAngle)* Mathf.Cos(playersAngle);
                verticalSpeedPercentage = 1 - horizontalSpeedPercentage;
                if (playersAngle == 0f)
                {
                    verticalSpeedPercentage = 1f;
                    horizontalSpeedPercentage = 0f;
                }
                break;
            case 4:
                verticalSpeedPercentage = Mathf.Sin(playersAngle)*Mathf.Sin(playersAngle);
                horizontalSpeedPercentage = 1 - verticalSpeedPercentage;
                if (playersAngle == 0f)
                {
                    verticalSpeedPercentage = 0f;
                    horizontalSpeedPercentage = 1f;
                }
                break;
        }
        WallsMovement.verticalSpeedPercentage = verticalSpeedPercentage;
        this.horizontalSpeedPercentage = horizontalSpeedPercentage*3;
        Debug.Log("angle case:"+angleCase.ToString()+" ver:"+verticalSpeedPercentage.ToString()+" hor:"+horizontalSpeedPercentage.ToString());



    }
    /// <summary>
    /// Checks if the player is dead
    /// </summary>
    /// <returns> True if the player is not alive</returns>
    public static bool Dead()
    {
        return Player.health == 0;
    }

    #endregion

    #region Timer Manager

    /// <summary>
    /// Manages all timer Updates and their events
    /// </summary>
    void manageTimers()
    {
        UpdateTimers();
        if (timerTicked(mAttackDelay))
        {
            ResetAttackDelay();
        }
        if (timerTicked(mAttackTime)) ResetAttack();
        
    }

    /// <summary>
    /// Checks if Timer Ticked
    /// </summary>
    /// <param name="timer">Current timer value</param>
    /// <returns>True if the timer ticked</returns>
    bool timerTicked(float timer)
    {
        return timer <= 0f;
    }

    /// <summary>
    /// Updates all timer inside class
    /// </summary>
    void UpdateTimers()
    {
        if (Attacking) mAttackTime -= Time.deltaTime;
        else if (mAttackedRecently) mAttackDelay -= Time.deltaTime;
    }
    #endregion

    #region Attack

    /// <summary>
    /// Sets all conditions to match attack
    /// </summary>
    void setUpAttack()
    {
        speed = AttackSpeed;
        Attacking = true;
        mAttackedRecently = true;
    }

    /// <summary>
    /// Returns condition to non-attacking
    /// </summary>
    void ResetAttack()
    {
        speed = speedNormal;
        Attacking = false;
        mAttackTime = AttackTime;
    }

    /// <summary>
    /// Resets delay between attacks
    /// </summary>
    void ResetAttackDelay()
    {
        mAttackedRecently = false;
        mAttackDelay = MaxAttackDelay;
    }
    #endregion
    
    #region Start&Update
    // Start is called before the first frame update
    void Start()
    {
        speed = speedNormal;
        mAttackDelay = MaxAttackDelay;
        mAttackTime = AttackTime;
    }
    // Update is called once per frame
    void Update()
    {
        manageTimers();
        if (Dead()) return;
        processInput();
        configureMovementSpeed();
    }
    #endregion

}
