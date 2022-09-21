using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnParent : MonoBehaviour
{
    private void Start()
    {
        transform.parent = null;
    }
}
