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
    [SerializeField] private Transform rack;

    protected override void Projectile()
    {
        if (bulletsLeft == 1)
        {
            animator.Play("Last shot");
            rack.localPosition = new Vector3(0, 0, -0.04f);
        }
        else
        {
            animator.Play("Shot");
        }
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
