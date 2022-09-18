using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponShooting : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private AudioSource[] sources;
    [SerializeField] private ParticleSystem flash;
    [SerializeField] private ParticleSystem pellets;
    [SerializeField] private Transform cam;
    [SerializeField] private float recoilRotation;
    [SerializeField] private float rotationTime;
    [SerializeField] private FirstPersonCamera test;
    [SerializeField] private Transform effectDad;
    [SerializeField] private Transform weaponTip;
    [SerializeField] private LayerMask bulletMask;
    [SerializeField] private GameObject decal;

    private int ammoCount = 2;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {

        //Debug.DrawRay(weaponTip.position, weaponTip.forward + new Vector3(testx, testy, 0));

        if (Input.GetKeyDown(KeyCode.Mouse0) && animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            if (ammoCount > 0)
            {
                ammoCount--;
                animator.Play("Shotgun shot");

                effectDad.position = weaponTip.position;
                effectDad.rotation = weaponTip.rotation;
                flash.Play();
                pellets.Play();

                sources[4].Play();
                sources[Random.Range(0, 2)].Play();

                StopAllCoroutines();
                StartCoroutine(Recoil());
                ShotsFired();
            }
            else
            {
                animator.Play("Dryfire");
                sources[4].Play();
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
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


    private void ShotsFired()
    {
        int pelletCout = 30;
        float maxDistance = 20;
        float maxAgle = 0.1f;
        RaycastHit hit;

        for (int i = 0; i < pelletCout; i++)
        {
            // raycasting
            Vector3 pelletDirection = cam.forward + new Vector3(Random.Range(-maxAgle, maxAgle), Random.Range(-maxAgle, maxAgle), Random.Range(-maxAgle, maxAgle));
            Physics.Raycast(cam.position, pelletDirection, out hit, maxDistance, bulletMask);
            
            // applying decal
            if (hit.point != null)
            {
                Instantiate(decal, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                // make enemies take damage
            }
        }
    }

    private void Reload()
    {
        ammoCount = 2;
        animator.Play("Reload");
        sources[3].Play();

    }
}
