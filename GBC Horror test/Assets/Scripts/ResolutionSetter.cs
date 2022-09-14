using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionSetter : MonoBehaviour
{
    [SerializeField] private Material displayMaterial;
    [SerializeField] [Range(1,100)] private int scalingFactor;
    private Texture display;

    private void Start()
    {
        display = displayMaterial.mainTexture;
        display.width = 16 * scalingFactor;
        display.height = 9 * scalingFactor;
        displayMaterial.SetTexture("Display Texture", display);
    }
}
