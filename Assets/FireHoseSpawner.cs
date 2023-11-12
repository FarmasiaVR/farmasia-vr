using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class FireHoseSpawner : MonoBehaviour
{
    public GameObject referenceObject;
    public GameObject objectToSpawnPrefab;
    public GameObject objectToDespawn;
    public GameObject objectToActivate;

    public void SpawnObject()
    {
        if (objectToDespawn != null)
        {
        objectToDespawn.SetActive(false);
        }
        if (referenceObject != null)
        {
            Vector3 spawnPosition = referenceObject.transform.position;
            Quaternion spawnRotation = referenceObject.transform.rotation;
            GameObject spawnedObject = Instantiate(objectToSpawnPrefab, spawnPosition, spawnRotation);
            spawnedObject.transform.localScale = referenceObject.transform.localScale;

        }
    }

    public void ActivateObject()
    {
        if (objectToDespawn != null)
        {
        objectToDespawn.SetActive(false);
        }
        if (objectToActivate != null)
        objectToActivate.SetActive(true);
    }
}

