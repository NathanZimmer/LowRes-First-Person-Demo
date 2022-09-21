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
        
    }
}
