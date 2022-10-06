using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponSM : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private BulletDecalController decalController;
    [SerializeField] private FirstPersonCamera cameraScript;
    [Header("Gameobjecs and Transforms")]
    [SerializeField] private GameObject bulletDecal;
    [SerializeField] private Transform weaponTip;
    [SerializeField] private Transform effectsParent;
    [SerializeField] protected Transform mainCam;
    [Header("Visual")]
    [SerializeField] private ParticleSystem bullets;
    [SerializeField] private ParticleSystem flash;
    [Header("Audio")]
    [SerializeField] private AudioSource[] shots;
    [SerializeField] private AudioSource fullReload;
    [SerializeField] private AudioSource partialReload;
    [SerializeField] private AudioSource dryfire;
    [Header("Weapon options")] 
    [SerializeField] private int magazineSize;
    [SerializeField] public bool infiniteAmmo;
    [SerializeField] protected LayerMask bulletsHit;
    [SerializeField] protected string enemyTag = "Enemy";
    [SerializeField] private float recoilRotation;
    [SerializeField] private float rotationTime;
    [SerializeField] bool chamberable; // if true, player can reload to magazineSize + 1

    private int bulletsLeft;
    protected Animator animator;
    [HideInInspector] public int weaponIndex;

    private void Start()
    {
        bulletsLeft = magazineSize;
    }

    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        animator.Play("Equip");
    }

    public void Shoot()
    {
        if (!infiniteAmmo)
            bulletsLeft--;

        if (bulletsLeft < 0)
            return;

        // moving effects to weapon tip and playing 
        effectsParent.SetPositionAndRotation(weaponTip.position, weaponTip.rotation);
        flash.Play();
        bullets.Play();

        // playing a random sound from the list of bullet sounds
        shots[Random.Range(0, shots.Length)].Play();

        // recoil
        StopCoroutine(Recoil());
        StartCoroutine(Recoil());

        // sending out projectile and adding decal
        Projectile();
    }

    public void Reload()
    {
        if (bulletsLeft > 0)
        {
            bulletsLeft = magazineSize + (chamberable ? 1 : 0);
        }
        else
        {
            bulletsLeft = magazineSize;
        }
    }

    public void ReloadSound()
    {
        if (bulletsLeft > 0)
        {
            partialReload.Play();
        }
        else
        {
            fullReload.Play();
        }
    }

    public int GetBulletsLeft() { return bulletsLeft; }

    public void PlayDryfire() { dryfire.Play(); }

    private IEnumerator Recoil()
    {
        float timeElapsed = 0;

        while (timeElapsed < rotationTime)
        {
            timeElapsed += Time.deltaTime;

            cameraScript.yRotation -= (recoilRotation / rotationTime) * Time.deltaTime;

            yield return null;
        }
    }

    protected void DrawDecals(RaycastHit hit)
    {
        decalController.CreateDecal(bulletDecal, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
    }

    protected abstract void Projectile();
}
