using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reload : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<WeaponSM>().ReloadSound();
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<WeaponSM>().Reload();
    }
}
