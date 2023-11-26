using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.XR.Interaction.Toolkit;

public class FireHoseSpawner : MonoBehaviour
{
    public GameObject referenceObject;
    public GameObject objectToSpawnPrefab;
    public GameObject objectToDespawn;
    public GameObject objectToActivate1;
    public GameObject objectToActivate2;
    public GameObject objectToActivate3;
    public GameObject head2;
    public GameObject head3;
    public GameObject headSpawn;
    public GameObject reel;
    public GameObject reelHalf;
    public GameObject reelEmpty;
    private int activeHose = 0;
    //public List<GameObject> hoseTwists1 = new List<GameObject>();
    public List<GameObject> hoseTwists2 = new List<GameObject>();
    public List<GameObject> hoseTwists3 = new List<GameObject>();
    public List<GameObject> twistPlaces = new List<GameObject>();
    public GameObject doorToLock;

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
            //TwistHose(hoseTwists1);
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
            SecureHead(head2);
        }
        else if (objectToActivate2 != null && activeHose == 2 && objectToActivate3 != null && reelHalf != null && reelEmpty != null)
        {
            // Door movement with attached spawned hose might cause problems with the longest hoselength
            if (doorToLock != null)
            {
                // Disable physics affecting the door
                if (doorToLock.GetComponent<Rigidbody>() != null)
                {
                    Rigidbody rb = doorToLock.GetComponent<Rigidbody>();
                    rb.isKinematic = true;
                }
                // Disable grabbing
                if (doorToLock.GetComponent<XRGrabInteractable>() != null)
                {
                    XRGrabInteractable grabInteractable = doorToLock.GetComponent<XRGrabInteractable>();
                    grabInteractable.interactionLayers = InteractionLayerMask.GetMask("Nothing");
                }
            }
            objectToActivate2.SetActive(false);
            reelHalf.SetActive(false);
            reelEmpty.SetActive(true);
            objectToActivate3.SetActive(true);
            activeHose = 3;
            TwistHose(hoseTwists3);
            SecureHead(head3);
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

    public void SecureHead(GameObject Head)
    {
    if (Head != null && headSpawn != null) { 
        Head.transform.position = headSpawn.transform.position;
        }
    }
}

