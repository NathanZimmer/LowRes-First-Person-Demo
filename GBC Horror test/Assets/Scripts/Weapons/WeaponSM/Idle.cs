using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : StateMachineBehaviour
{
    [SerializeField] private bool limitRelaod = false;
    [SerializeField] private int minBullets;

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int bulletsLeft = animator.GetComponent<WeaponSM>().GetBulletsLeft();

        if (bulletsLeft <= 0)
        {
            animator.Play("Empty Idle");
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (bulletsLeft == 1)
                animator.Play("Last Shot");
            else
                animator.Play("Shot");
        }
        else if (Input.GetKeyDown(KeyCode.R) && (!limitRelaod || bulletsLeft <= minBullets))
        {
            animator.Play("Reload");
        }
    }
}
