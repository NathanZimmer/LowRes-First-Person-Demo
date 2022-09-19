using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalRemoval : MonoBehaviour
{
    public int decalIndex;
    public int maxDecals;
    private weaponShooting shooting;

    private void Awake()
    {
        shooting = GameObject.Find("shotgun").GetComponent<weaponShooting>();
    }

    void Update()
    {
        if (shooting.decalNum > maxDecals)
        {
            if (decalIndex == 1)
            {
                Destroy(gameObject);
                shooting.decalNum--;
            }
            else
            {
                decalIndex--;
            }
        }    
    }
}
