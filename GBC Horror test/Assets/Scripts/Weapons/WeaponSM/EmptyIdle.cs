using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyIdle : StateMachineBehaviour
{
    private bool exiting;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        exiting = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0)
        {
            animator.Play("Unequip");
            exiting = true;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            animator.Play("Empty Reload");
            exiting = true;
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.Play("Dryfire");
            exiting = true;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!exiting)
            animator.Play("Empty Idle");
    }
}
