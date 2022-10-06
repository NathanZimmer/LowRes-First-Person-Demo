using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : StateMachineBehaviour
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetComponent<WeaponSM>().GetBulletsLeft() <= 0)
        {
            animator.Play("Empty Idle");
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0)
        {
            animator.Play("Unequip");
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (animator.GetComponent<WeaponSM>().GetBulletsLeft() == 1)
                animator.Play("Last Shot");
            else
                animator.Play("Shot");
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            animator.Play("Reload");
        }
    }
}
