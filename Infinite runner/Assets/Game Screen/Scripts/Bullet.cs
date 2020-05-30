using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using static WallsMovement;
public class Bullet : MonoBehaviour
{
    #region Public Properties
    public bool fired; // true if the bullet is fired
    
    public float speed; // bullet speed

    #endregion

    #region Private Members

    Vector3 moveTowards; // Players position when the bullet was instantiated
    Vector3 startingPosition;// bullets starting position which is assigned to enemy spawner
    Rigidbody bullet; // Rigid body used to add force to the bullet

    #endregion

    #region Start&Update
    void Start()
    {
        bullet = GetComponent<Rigidbody>();
        bullet.detectCollisions = true;
        bullet.useGravity = false;
        startingPosition = transform.position;
        moveTowards = GameObject.Find("Player").GetComponent<Transform>().position;
        moveTowards.y += 0.2f;
        fired = true;
    }
    void Update()
    {

        transform.LookAt(2 * startingPosition - moveTowards);
        movement();
        if (fired)
        {
            bullet.velocity = (moveTowards - transform.position).normalized * speed * Time.deltaTime;
            fired = false;
        }
    }
    #endregion

    #region Bullet Movement //Interface implementation needed

    /// <summary>
    /// Refers bullets position compared to the player
    /// </summary>
    void movement()
    {
        if (Input.GetKey(KeyCode.S) && !collidingBackward)
        {
            transform.Translate(Vector3.left * Time.deltaTime * WallsMovement.speed
                                * verticalSpeedPercentage, Space.World); //Backward
        }
        else if (Input.GetKey(KeyCode.W) && !collidingForward)
        {
            transform.Translate(Vector3.right * Time.deltaTime
                                * verticalSpeedPercentage * WallsMovement.speed, Space.World); //Forward

        }

    }

    #endregion

    #region Colliders
    /// <summary>
    /// Calls every time Bullet collider hits something
    /// </summary>
    /// <param name="collision"> The object which bullet countered</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Enemy")
        {
            Destroy(gameObject);
        }
    }
    #endregion
}
