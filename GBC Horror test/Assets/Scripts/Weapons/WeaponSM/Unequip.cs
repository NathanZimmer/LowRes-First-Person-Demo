using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unequip : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        WeaponSelection selection = GameObject.Find("Player").GetComponent<WeaponSelection>();

        
    }
}
