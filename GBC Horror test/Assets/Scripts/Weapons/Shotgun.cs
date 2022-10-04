using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapons
{
    [Header("Projectile Settings")]
    [SerializeField] private int pelletCount;
    [SerializeField] private float spreadAngle;
    [SerializeField] private float maxDistance;

    protected override void Projectile()
    {
        animator.Play("Shot");
        RaycastHit hit;

        for (int i = 0; i < pelletCount; i++)
        {
            // raycasting
            Vector3 pelletDirection = mainCam.forward + new Vector3(Random.Range(-spreadAngle, spreadAngle), Random.Range(-spreadAngle, spreadAngle), Random.Range(-spreadAngle, spreadAngle));
            Physics.Raycast(mainCam.position, pelletDirection, out hit, maxDistance, bulletsHit);

            // applying decal
            if (hit.point != null)
            {
                drawDecals(hit);
                if (hit.transform.CompareTag("Enemy"))
                    hit.transform.gameObject.GetComponent<EnemyTest>().CalculateHits(30);
            }
        }
    }
}
