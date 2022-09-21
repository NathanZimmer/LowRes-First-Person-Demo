using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    private Material defaultMaterial;
    private float damage;

    private void Start()
    {
        defaultMaterial = GetComponent<Renderer>().material;
    }

    private void LateUpdate()
    {
        GetHit(damage);
    }

    public void CalculateHits(float damage)
    {
        StopAllCoroutines();
        this.damage += damage;
    }

    private void GetHit(float damage)
    { 
        if (damage < 1) { }
        else if (damage < 25)
        {
            StartCoroutine(SetMaterialColor(Color.green));
        }
        else if (damage < 75)
        {
            StartCoroutine(SetMaterialColor(Color.yellow));
        }
        else
        {
            StartCoroutine(SetMaterialColor(Color.red));
        }
    }


    private IEnumerator SetMaterialColor(Color color)
    {
        defaultMaterial.color = color;

        yield return new WaitForSeconds(2);

        defaultMaterial.color = Color.white;
        damage = 0;
    }
}
