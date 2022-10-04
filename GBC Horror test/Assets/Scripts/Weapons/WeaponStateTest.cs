using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStateTest : MonoBehaviour
{
    public int magazineSize;
    public int bulletsLeft;
    public LayerMask bulletsHit;
    public BulletDecalController decalController;
    public GameObject bulletDecal;
    public Transform weaponTip;
    public Transform effects;
    public ParticleSystem bullets;
    public ParticleSystem flash;
    public AudioSource shotSFX;
    public AudioSource reloadSFX;
    public AudioSource partialReloadSFX;

    public void Projectile()
    {
        bulletsLeft--;

        RaycastHit hit;
        Physics.Raycast(transform.position, transform.forward, out hit, 100, bulletsHit);

        effects.position = weaponTip.position;
        effects.rotation = weaponTip.rotation;
        flash.Play();
        bullets.Play();
        shotSFX.Play();


        // applying decal
        if (hit.point != null)
        {
            drawDecals(hit);
            if (hit.transform.CompareTag("Enemy"))
                hit.transform.gameObject.GetComponent<EnemyTest>().CalculateHits(30);
        }
    }
    protected void drawDecals(RaycastHit hit)
    {
        decalController.CreateDecal(bulletDecal, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
    }

    public void Reload(bool extraBullet)
    {
        if (extraBullet)
        {
            bulletsLeft = magazineSize + 1;
            partialReloadSFX.Play();
        }
        else
        {
            bulletsLeft = magazineSize;
            reloadSFX.Play();
        }
    }
}
