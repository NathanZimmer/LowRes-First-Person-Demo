using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapons : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] protected AudioSource[] shots;
    [SerializeField] protected AudioSource reload;
    [SerializeField] protected AudioSource dryfire;
    [Header("Visuals")]
    [SerializeField] protected ParticleSystem flash;
    [SerializeField] protected ParticleSystem bullets;
    [Header("Settings")]
    [SerializeField] protected float recoilRotation;
    [SerializeField] protected float rotationTime;
    [SerializeField] protected BulletDecalController decalController;
    [SerializeField] protected GameObject bulletDecal;
    [SerializeField] protected LayerMask bulletsHit;
    [SerializeField] protected int magazineSize;
    [SerializeField] protected bool infiniteAmmo;
    [Header("Other")]
    [SerializeField] protected Transform mainCam;
    [SerializeField] protected FirstPersonCamera cameraScript;
    [SerializeField] protected Transform effectParent;
    [SerializeField] protected Transform weaponTip;
    [SerializeField] protected WeaponSway weaponSway;

    protected int bulletsLeft;
    protected Animator animator; // only supports animations titled: "Idle" "Shot" "Reload" and "Dryfire"
    private bool reloading = false;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        bulletsLeft = magazineSize;
    }

    private void OnEnable()
    {
        if (reloading)
        {
            Reload();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            if (bulletsLeft > 0 || infiniteAmmo)
            {
                // moving effects to weapon tip and playing 
                effectParent.position = weaponTip.position;
                effectParent.rotation = weaponTip.rotation;
                flash.Play();
                bullets.Play();

                // playing a random sound from the list of bullet sounds
                shots[Random.Range(0, shots.Length)].Play();

                StopAllCoroutines();
                StartCoroutine(Recoil());
                Projectile();
                bulletsLeft--;
            }
            else
            {
                // playing sfx and animation for dry firing gun
                animator.Play("Dryfire");
                dryfire.Play();
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        reloading = animator.GetCurrentAnimatorStateInfo(0).IsName("Reload");
    }

    private IEnumerator Recoil()
    {
        float timeElapsed = 0;
        float originalY = cameraScript.yRotation;

        while (timeElapsed < rotationTime)
        {
            timeElapsed += Time.deltaTime;

            cameraScript.yRotation -= (recoilRotation / rotationTime) * Time.deltaTime;

            yield return null;
        }
    }

    protected void drawDecals(RaycastHit hit)
    {
        decalController.CreateDecal(bulletDecal, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
    }

    private void Reload()
    {
        bulletsLeft = magazineSize;
        animator.Play("Reload");
        reload.Play();
    }

    protected abstract void Projectile();
}
