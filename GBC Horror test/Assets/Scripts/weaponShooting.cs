using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponShooting : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private ParticleSystem flash;
    [SerializeField] private ParticleSystem pellets;

    private void Start()
    {
        animator = GetComponent<Animator>();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.Play("Shotgun shot");
            flash.Play();
            //pellets.Play();
        }
    }
}
