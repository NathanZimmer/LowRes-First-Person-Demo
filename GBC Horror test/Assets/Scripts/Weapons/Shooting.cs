using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}

    private bool repeatAnimation = false;
    private bool reload = false;
    private bool lastShot = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        repeatAnimation = false;
        reload = false;
        lastShot = false;
        animator.gameObject.GetComponent<WeaponStateTest>().Projectile();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (animator.gameObject.GetComponent<WeaponStateTest>().bulletsLeft == 1)
                lastShot = true;
            else 
                repeatAnimation = true;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            reload = true;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (lastShot)
            animator.Play("Last Shot");
        else if (repeatAnimation)
            animator.Play("Shooting");
        else if (reload)
            animator.Play("Reloading");
    }
}
