using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKarmsController : MonoBehaviour
{
    [SerializeField] private Transform rhPoint;
    [SerializeField] private Transform rePoint;
    [SerializeField] private Transform lhPoint;
    [SerializeField] private Transform lePoint;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        // right hand
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        animator.SetIKPosition(AvatarIKGoal.RightHand, rhPoint.position);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
        animator.SetIKRotation(AvatarIKGoal.RightHand, rhPoint.rotation);
        animator.SetIKHintPosition(AvatarIKHint.RightElbow, rePoint.position);
        animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, 1);

        // left hand
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 100);
        animator.SetIKPosition(AvatarIKGoal.LeftHand, lhPoint.position);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, lhPoint.rotation);
        animator.SetIKHintPosition(AvatarIKHint.LeftElbow, lePoint.position);
        animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, 1);
    }
}
