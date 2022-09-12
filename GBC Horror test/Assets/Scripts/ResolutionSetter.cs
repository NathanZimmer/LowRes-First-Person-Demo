using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionSetter : MonoBehaviour
{
    [SerializeField] private Material displayMaterial;
    [SerializeField] [Range(1,100)] private int scalingFactor;
    private float oldFactor;
    private Texture display;

    private void Start()
    {
        oldFactor = scalingFactor;
        display = displayMaterial.mainTexture;
    }

    private void Update()
    {
        if (scalingFactor != oldFactor)
        { 
            display.width = 16 * scalingFactor;
            display.height = 9 * scalingFactor;

            oldFactor = scalingFactor;
            displayMaterial.SetTexture("Display Texture", display);
        }
    }
}
