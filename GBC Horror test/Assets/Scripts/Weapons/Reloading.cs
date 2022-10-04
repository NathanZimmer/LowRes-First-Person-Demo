using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reloading : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<WeaponStateTest>().Reload(animator.GetCurrentAnimatorStateInfo(0).IsName("Reloading"));
    }
}
