using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapons
{
    [Header("Projectile Settings")]
    [SerializeField] private float maxDistance;
    [SerializeField] private float spreadAngle;
    [SerializeField] private float spreadIncreaseRate;
    [SerializeField] private float spreadDecreaseRate;

    protected override void Projectile()
    {
        RaycastHit hit;

        Physics.Raycast(mainCam.position, mainCam.forward, out hit, maxDistance, bulletsHit);

        // applying decal
        if (hit.point != null)
        {
            drawDecals(hit);
            // if hit is an enemy do something
        }
    }
}
