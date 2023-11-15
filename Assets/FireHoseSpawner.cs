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
    public GameObject objectToActivate3;
    public GameObject reel;
    public GameObject reelHalf;
    public GameObject reelEmpty;
    private int activeHose = 0;
    public List<GameObject> hoseTwists2 = new List<GameObject>();
    public List<GameObject> hoseTwists3 = new List<GameObject>();
    public List<GameObject> twistPlaces = new List<GameObject>();

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
        if (objectToDespawn != null && activeHose == 0 && objectToActivate1 != null)
        {
            //Destroy(objectToDespawn);
            objectToDespawn.SetActive(false);
            objectToActivate1.SetActive(true);
            activeHose = 1;
        }
        else if (objectToActivate1 != null && activeHose == 1 && objectToActivate2 != null && reelHalf != null && reel != null)
        {
            //Destroy(objectToActivate1);
            objectToActivate1.SetActive(false);
            reel.SetActive(false);
            reelHalf.SetActive(true);
            objectToActivate2.SetActive(true);
            activeHose = 2;
            TwistHose(hoseTwists2);
        }
        else if (objectToActivate2 != null && activeHose == 2 && objectToActivate3 != null && reelHalf != null && reelEmpty != null)
        {
            objectToActivate2.SetActive(false);
            reelHalf.SetActive(false);
            reelEmpty.SetActive(true);
            objectToActivate3.SetActive(true);
            activeHose = 3;
            TwistHose(hoseTwists3);
        }
    }
    public void TwistHose(List<GameObject> hoseTwists)
    {
        int index = 0;
        if (hoseTwists != null && twistPlaces != null) { 
            foreach (GameObject item1 in hoseTwists)
            {
                GameObject item2 = twistPlaces[index];
                item1.transform.position = item2.transform.position;
                index++;
            }
        }
    }
}

