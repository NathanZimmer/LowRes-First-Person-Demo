using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : StateMachineBehaviour
{
    private bool repeatAnimation;
    private bool reload;
    private bool lastShot;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        repeatAnimation = false;
        reload = false;
        lastShot = false;

        animator.GetComponent<WeaponSM>().Shoot();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (animator.GetComponent<WeaponSM>().GetBulletsLeft() == 1)
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
            animator.Play("Shot");
        else if (reload)
            animator.Play("Reload");
    }
}
