using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueStickSpawner : MonoBehaviour
{
    public string itemTag = "Bluestick";
    public GameObject Bluestick;
    public float checkInterval = 2f;
    public Transform ItemSpawnPoint;
    
    private Collider areaCollider;

    void Start()
    {
        areaCollider = GetComponent<Collider>();
        InvokeRepeating(nameof(CheckAndSpawnItem), 1f, checkInterval);
    }

    void CheckAndSpawnItem()
    {
        Collider[] colliders = Physics.OverlapBox(
            areaCollider.bounds.center,
            areaCollider.bounds.extents,
            Quaternion.identity);

        bool itemFound = false;
        foreach (var col in colliders)
        {
            if (col.CompareTag(itemTag))
            {
                itemFound = true;
                break;
            }
        }

        if (!itemFound)
        {
            GameObject clone = Instantiate(Bluestick, ItemSpawnPoint.position, ItemSpawnPoint.rotation);
            clone.SetActive(true);
        }
    }

}