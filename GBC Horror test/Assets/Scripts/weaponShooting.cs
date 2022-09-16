using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponShooting : MonoBehaviour
{
    private Animator animator;
    private AudioSource shot;
    [SerializeField] private ParticleSystem flash;
    [SerializeField] private ParticleSystem pellets;
    [SerializeField] private Transform cam;
    [SerializeField] private float recoilRotation;
    [SerializeField] private float rotationTime;
    [SerializeField] private FirstPersonCamera test;

    private void Start()
    {
        animator = GetComponent<Animator>();
        shot = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            animator.Play("Shotgun shot");
            flash.Play();
            shot.Play();

            StopAllCoroutines();
            StartCoroutine(Recoil());
        }
    }

    private IEnumerator Recoil()
    {
        float timeElapsed = 0;
        float originalY = test.yRotation;

        while (timeElapsed < rotationTime)
        {
            timeElapsed += Time.deltaTime;
            float normTime = timeElapsed / rotationTime;

            test.yRotation -= Mathf.Lerp(test.yRotation, recoilRotation, normTime);

            yield return null;
        }

        /*
        yield return new WaitForSeconds(0.2f);

        while(test.yRotation < originalY - 1)
        {
            timeElapsed += Time.deltaTime;
            float normTime = timeElapsed / (rotationTime * 10);

            test.yRotation += Mathf.Lerp(test.yRotation, originalY, normTime);

            yield return null;
        }
        */
    }
}
