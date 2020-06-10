using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Assertions.Must;
using  static WallsMovement;

public class Enemy : MonoBehaviour
{

    [Header("Spawner responsible for bullets")]
    public Transform pPosition;//position to shoot at
    public Animator enemyAnimator;//animator resposible for throwing animations
    public float Throw = 0f;//Animation state
    public float range = 5f;//Shooting range
    private bool haveBullet;
    public Transform spawnerPosition;
    public GameObject bullet;

    private bool alive;

    [Header("Shooting  Timer")]
    private float delay; //Specific shooting delay for each enemy
    static float startingDelay = 2f;//Delay between two shots;



    #region Start&Update
    void Start()
    {
        pPosition = GameObject.Find("Player").transform;
        enemyAnimator = this.gameObject.GetComponent<Animator>();
        delay = -1f;
        haveBullet = false;
        alive = true;
    }

    // Update is called once per frame
    void Update()
    {
        destroyChecker();
        if (!alive) return;
        if (Player.health == 0)
        {

            enemyAnimator.SetFloat("Throw", 0f);
            return;
        }
        movement();
        
        if(!inRange()||Player.health==0f) enemyAnimator.SetFloat("Throw", 0f);

        CheckShoting();
        if (!haveBullet && UpdateTimer())
        ResetEnemy();
    }
    #endregion

    /// <summary>
    /// Resets enemys stats to the starting ones 
    /// </summary>
    private void ResetEnemy()
    {
        Throw = 0;
        haveBullet = true;
        delay = startingDelay;
    }

    /// <summary>
    /// Transforms enemy so it look at the player
    /// </summary>
    private void LookTo()
    {
        var targetPosition = pPosition.position;
        targetPosition.y = transform.position.y;
        transform.LookAt(targetPosition);
    }


    /// <summary>
    /// Controls enemys position compared to the players
    /// </summary>
    void movement()
    {
        if (Input.GetKey(KeyCode.S) && !collidingBackward)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed
                                * verticalSpeedPercentage, Space.World); //Backward
            if (objectStill) objectStill = false;
        }
        else if (Input.GetKey(KeyCode.W) && !collidingForward)
        {
            transform.Translate(Vector3.right * Time.deltaTime
                                * verticalSpeedPercentage * speed, Space.World); //Forward
            if (objectStill) objectStill = false;
        }
        else if (!objectStill)
            objectStill = true;
        
    }
    /// <summary>
    /// Checks if the enemy has to die on his own
    /// </summary>
    private void destroyChecker()
    {
        if (transform.position.x > 15 || transform.position.y < -5f)
            Destroy(gameObject);
    }
    /// <summary>
    /// Checks if the enemy has to shoot
    /// </summary>
    void CheckShoting()
    {
        if (inRange())
        {
            LookTo();

            RaycastHit hit;
            if (Physics.Linecast(transform.position, pPosition.position, out hit))
            {
                if (hit.transform.tag == "Player" && haveBullet)
                {
                    
                    enemyAnimator.SetFloat("Throw", 1f);
                    if(enemyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime-0.3f<0.1e9)
                    {
                        Shoot();
                        haveBullet = false;
                    }
                    
                }
                
            }
        }
    }

    /// <summary>
    /// Checks if the player is in range
    /// </summary>
    /// <returns>True if the player is in range</returns>
    bool inRange()
    {
        return Mathf.Abs(transform.position.x - pPosition.position.x) <= range;
    }

    /// <summary>
    /// Updates timer and check if it ticked
    /// </summary>
    /// <returns>True if the timer ticked</returns>
    bool UpdateTimer()
    {
        if (delay <= 0f) return true;//Time elapsed;
        else delay -= Time.deltaTime;
        return false;                  //timer is still going
    }

    /// <summary>
    /// Spawns a bullet
    /// </summary>
    public void Shoot()
    {
        Vector3 spawnPos = spawnerPosition.position;
        spawnPos.y += 0.3f;
        Instantiate(bullet, spawnPos,transform.rotation);
    }


    #region Collision

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && Player.Attacking && alive)
        {
            CountdownTimer.addExtraTime();
            ScoreCounter.addExtraScore();
            enemyAnimator.SetTrigger("Die");
            Destroy(gameObject, 1f);
            alive = false;
        }
            
            
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && Player.Attacking && alive)
        {
            CountdownTimer.addExtraTime();
            ScoreCounter.addExtraScore();
            enemyAnimator.SetTrigger("Die");
            Destroy(gameObject, 1f);
            alive = false;
        }
    }
    #endregion
}
