using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelection : MonoBehaviour
{
    [Header("Weapon 1")]
    [SerializeField] private GameObject weapon1;
    [SerializeField] private Transform w1RhPoint;
    [SerializeField] private Transform w1LhPoint;
    [SerializeField] private Transform w1RePoint;
    [SerializeField] private Transform w1LePoint;
    [Header("Weapon 2")]
    [SerializeField] private GameObject weapon2;
    [SerializeField] private Transform w2RhPoint;
    [SerializeField] private Transform w2LhPoint;
    [SerializeField] private Transform w2RePoint;
    [SerializeField] private Transform w2LePoint;
    [Header("Other")]
    [SerializeField] [Range(1,3)] private int defaultSelected;
    [SerializeField] private IKarmsController ikController;

    private GameObject selectedWeapon = null;

    private void Start()
    {
        weapon1.GetComponentInChildren<WeaponSM>().weaponIndex = 1;
        weapon2.GetComponentInChildren<WeaponSM>().weaponIndex = 2;

        switch (defaultSelected) 
        {
            case 1:
                EquipWeapon(weapon1, w1RhPoint, w1RePoint, w1LhPoint, w1LePoint);
                selectedWeapon = weapon1;
                break;
            case 2:
                EquipWeapon(weapon2, w2RhPoint, w2RePoint, w2LhPoint, w2LePoint);
                selectedWeapon = weapon2;
                break;
            case 3:
                selectedWeapon = null;
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (selectedWeapon == weapon1)
            {
                Unequip(weapon1);
                selectedWeapon = null;
            }
            else
            {
                Unequip(weapon2);
                EquipWeapon(weapon1, w1RhPoint, w1RePoint, w1LhPoint, w1LePoint);
                selectedWeapon = weapon1;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (selectedWeapon == weapon2)
            {
                Unequip(weapon2);
                selectedWeapon = null;
            }
            else
            {
                Unequip(weapon1);
                EquipWeapon(weapon2, w2RhPoint, w2RePoint, w2LhPoint, w2LePoint);
                selectedWeapon = weapon2;
            }
        }
    }

    private void EquipWeapon(GameObject weapon, Transform rhPoint, Transform rePoint, Transform lhPoint, Transform lePoint)
    {
        weapon.SetActive(true);
        ikController.SetIKTransforms(rhPoint, rePoint, lhPoint, lePoint);
    }

    private void Unequip(GameObject weapon)
    {
        weapon.SetActive(false);
        ikController.ResetIKTransforms();
    }
}
