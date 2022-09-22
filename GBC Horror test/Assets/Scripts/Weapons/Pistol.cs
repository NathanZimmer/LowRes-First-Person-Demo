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

    // Vector3(0.299499989,-0.381999999,0.783100009)
    // rh:
    // Vector3(0.0227000006,0.063699998,-0.0344000012)
    // Quaternion(-0.146600559,-0.0304065906,-0.525117159,0.837756336)
    // lh:
    // Vector3(-0.0197999999,0.0386000015,-0.0357999913)
    // Quaternion(0.0662081093,0.0933062807,0.778103471,0.617628872)
    // re:
    // Vector3(0.171000004,-0.284999996,-0.356999844)
    // le: 
    // Vector3(-0.437000006,-0.444000006,-0.407000005)

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
