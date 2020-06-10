using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerAnimator : MonoBehaviour
{
    public Animator animator;
    bool mDeathAnimationSet;
    void Start()
    {
        mDeathAnimationSet = false;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mDeathAnimationSet) return;
        if(Player.health == 0)
        {
            mDeathAnimationSet = true;
            animator.SetTrigger("Death");
            return;
        }
            

        if (Input.GetKey(KeyCode.W))
            animator.SetBool("isRunning", true);
        else animator.SetBool("isRunning", false);
        if (Input.GetKey(KeyCode.Space)&&Player.Attacking)
            animator.SetTrigger("Attack");
        #region Idle part
        if (Input.GetKey(KeyCode.D))
            animator.SetFloat("Turn", 1f);
        else if (Input.GetKey(KeyCode.A))
            animator.SetFloat("Turn", 0f);
        else animator.SetFloat("Turn",0.5f);
        #endregion

        if(Input.GetKey(KeyCode.S))
            animator.SetBool("WalkingBackward", true);
        else animator.SetBool("WalkingBackward", false);
    }
}
