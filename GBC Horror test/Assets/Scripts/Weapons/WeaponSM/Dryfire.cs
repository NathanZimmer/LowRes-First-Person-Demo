using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dryfire : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<WeaponSM>().PlayDryfire();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.Play("Empty Idle");
    }
}
