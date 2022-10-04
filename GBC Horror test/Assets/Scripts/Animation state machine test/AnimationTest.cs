using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    public string test;
    public string test2;
    private string currentAnimation;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        currentAnimation = test;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentAnimation = test;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentAnimation = test2;
        }

        animator.Play(currentAnimation);
    }
}
