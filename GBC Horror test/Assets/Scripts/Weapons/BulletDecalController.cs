using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDecalController : MonoBehaviour
{
    [SerializeField] private int decalLimit;
    private LinkedList<GameObject> decalList;

    private void Awake()
    {
        decalList = new LinkedList<GameObject>();
    }

    private void LateUpdate()
    {
        if (decalList != null && decalList.Count > decalLimit)
        {
            DeleteDecal();
        }
    }

    public void CreateDecal(GameObject decal, Vector3 position, Quaternion rotation)
    {
        GameObject newDecal = Instantiate(decal);
        newDecal.transform.position = position;
        newDecal.transform.rotation = rotation;

        decalList.AddLast(newDecal);
    }

    private void DeleteDecal()
    {
        GameObject removed = decalList.First.Value;
        decalList.Remove(removed);
        Destroy(removed);
    }
}
