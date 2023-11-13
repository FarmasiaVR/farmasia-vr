using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class FireHoseSpawner : MonoBehaviour
{
    public GameObject referenceObject;
    public GameObject objectToSpawnPrefab;
    public GameObject objectToDespawn;
    public GameObject objectToActivate1;
    public GameObject objectToActivate2;
    public GameObject reel;
    public GameObject reelHalf;
    public GameObject reelEmpty;
    private int activeHose = 0;

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
        if (objectToDespawn != null && activeHose == 0 && objectToActivate1 != null && reel != null && reelHalf != null)
        {
            //Destroy(objectToDespawn);
            objectToDespawn.SetActive(false);
            reel.SetActive(false);
            reelHalf.SetActive(true);
            objectToActivate1.SetActive(true);
            activeHose = 1;
        }
        else if (objectToActivate1 != null && activeHose == 1 && objectToActivate2 != null && reelHalf != null && reelEmpty != null)
        {
            //Destroy(objectToActivate1);
            objectToActivate1.SetActive(false);
            reelHalf.SetActive(false);
            reelEmpty.SetActive(true);
            objectToActivate2.SetActive(true);
            activeHose = 2;
        }
    }
}

