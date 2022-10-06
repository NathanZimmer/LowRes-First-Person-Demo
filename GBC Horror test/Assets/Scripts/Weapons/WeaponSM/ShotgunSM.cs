using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunSM : WeaponSM
{
    [Header("Projectile Settings")]
    [SerializeField] private int pelletCount;
    [SerializeField] private float pelletDamage;
    [SerializeField] private float spreadAngle;
    [SerializeField] private float maxDistance;

    protected override void Projectile()
    {
        for (int i = 0; i < pelletCount; i++)
        {
            // raycasting
            Vector3 pelletDirection = mainCam.forward + mainCam.TransformDirection(new Vector3(Random.Range(-spreadAngle, spreadAngle), Random.Range(-spreadAngle, spreadAngle), 0));
            Physics.Raycast(mainCam.position, pelletDirection, out RaycastHit hit, maxDistance, bulletsHit);

            // applying decal
            if (hit.point != null)
            {
                DrawDecals(hit);

                if (hit.transform.CompareTag(enemyTag))
                {
                    hit.transform.gameObject.GetComponent<EnemyTest>().CalculateHits(pelletDamage);
                }
            }
        }
    }
}