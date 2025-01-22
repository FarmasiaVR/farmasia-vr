using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.XR.Interaction.Toolkit;

//This script is responsible for "spawning" different lengths of firehose. The "spawn" is done by activation or disabling of gameobjects.
//There also exists a method for truly spawning a new hose but it was left unused due to other difficulties with this kind of way to get longer
//hoses for the firehosesystem. However it still is functional.
//As total she script is responsible of activating new hose and diabling the old one. It also changes to hosereel model to fit the new hose.
//The hoses are twisted on spawn so they dont accidentally end inside other gameobjects.

public class FireHoseSpawner : MonoBehaviour
{
    //spawn location for the no longer used spawner.
    public GameObject referenceObject;
    //spawned object
    public GameObject objectToSpawnPrefab;
    //object to replace with the spawned object.
    public GameObject objectToDespawn;
    //references to the at start intactive hoses.
    public GameObject objectToActivate1;
    public GameObject objectToActivate2;
    public GameObject objectToActivate3;
    //references to hose heads for 2 longest hoseversions.
    public GameObject head2;
    public GameObject head3;
    //location to move the head of activated hose.
    public GameObject headSpawn;
    //references to reel gameobjects with different models.
    public GameObject reel;
    public GameObject reelHalf;
    public GameObject reelEmpty;
    //parameter to keep track of the now active hose.
    private int activeHose = 0;
    //lists of twist locations needed to bend the hose to its starting shape.
    //public List<GameObject> hoseTwists1 = new List<GameObject>();
    public List<GameObject> hoseTwists2 = new List<GameObject>();
    public List<GameObject> hoseTwists3 = new List<GameObject>();
    public List<GameObject> twistPlaces = new List<GameObject>();
    //reference to cabinet door as if its moved when the hose is at full length the physics go wild.
    public GameObject doorToLock;

    public void SpawnObject()
    //no longer used but still functional, spawns determined object to determined location and deactivates old object.
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
    //used for activating the different hose lengths. Also calls methods for twisting the hose and managing the hosehead location.
    {
        //first activation
        if (objectToDespawn != null && activeHose == 0 && objectToActivate1 != null)
        {
            //Destroy(objectToDespawn);
            objectToDespawn.SetActive(false);
            objectToActivate1.SetActive(true);
            activeHose = 1;
            //TwistHose(hoseTwists1);
        }
        //second activation
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
        //third activation, the cabinet door will be also made immovable to physics and unable to ge grabbed and moved by the player.
        else if (objectToActivate2 != null && activeHose == 2 && objectToActivate3 != null && reelHalf != null && reelEmpty != null)
        {
            // This is done because door movement with attached spawned hose might cause problems with the longest hoselength.
            if (doorToLock != null)
            {
                // Disable physics affecting the door
                if (doorToLock.GetComponent<Rigidbody>() != null)
                {
                    Rigidbody rb = doorToLock.GetComponent<Rigidbody>();
                    rb.isKinematic = true;
                }
                // Disable grabbing by changing the layermask.
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
    //twists hose object to shape determined by given gameobject locations.
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
    //moves the hosehead to a determined position so it most likely ends in easily reachable place.
    {
    if (Head != null && headSpawn != null) { 
        Head.transform.position = headSpawn.transform.position;
        }
    }
}

