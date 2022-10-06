using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastShot : StateMachineBehaviour
{
    private bool reload;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        reload = false;

        animator.GetComponent<WeaponSM>().Shoot();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            reload = true;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (reload)
            animator.Play("Empty Reload");
        else
            animator.Play("Empty Idle");
    }
}
