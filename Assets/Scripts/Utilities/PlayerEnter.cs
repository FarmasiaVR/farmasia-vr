using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnter : MonoBehaviour
{
    private bool playerEntered;
    private bool playerExited;
    private bool playerInside;
    private bool enteredOnce;
    private Material startingMaterial;
    [SerializeField]
    Material enteredMaterial;

    [SerializeField]
    float greenShownTime;

    private MeshRenderer meshRenderer;
    private GameObject emergencyShowerButton, fireExtinguisherButton, eyeShowerButton, fireBlanketButton;
    enum Scenario
    {
        EmergencyShower,
        FireExtinguisher,
        EyeShower,
        FireBlanket
    };

    [SerializeField]
    Scenario scenario = new Scenario();

    // Start is called before the first frame update
    void Start()
    {
        emergencyShowerButton = GameObject.Find("TutorialLocations").transform.Find("TeleportButtons/TutorialTeleporter EmergencyShower").gameObject;
        fireExtinguisherButton = GameObject.Find("TutorialLocations").transform.Find("TeleportButtons/TutorialTeleporter FireExtinguisher").gameObject;
        eyeShowerButton = GameObject.Find("TutorialLocations").transform.Find("TeleportButtons/TutorialTeleporter EyeShower").gameObject;
        fireBlanketButton = GameObject.Find("TutorialLocations").transform.Find("TeleportButtons/TutorialTeleporter FireBlanket").gameObject;
        startingMaterial = gameObject.GetComponent<MeshRenderer>().material;
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        playerEntered = false;
        playerInside = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerEntered)
        {
            greenShownTime -= Time.deltaTime;
            if (greenShownTime <= 0)
            {
                meshRenderer.material = startingMaterial;
            }
            if (this.gameObject.name == "PlayerEnterBox EmergencyShower")
            {
                meshRenderer.material = enteredMaterial;
                emergencyShowerButton.SetActive(true);
            }
            if (this.gameObject.name == "PlayerEnterBox FireBlanket")
            {
                meshRenderer.material = enteredMaterial;
                fireBlanketButton.SetActive(true);
            }
            if (this.gameObject.name == "PlayerEnterBox FireExtinguisher")
            {
                meshRenderer.material = enteredMaterial;
                fireExtinguisherButton.SetActive(true);
            }
            if (this.gameObject.name == "PlayerEnterBox EyeShower")
            {
                meshRenderer.material = enteredMaterial;
                eyeShowerButton.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerCollider")
        {
            playerEntered = true;
            playerInside = true;
            enteredOnce = true;
            Debug.Log("Player entered the " + scenario + " area");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "PlayerCollider")
        {
            playerExited = true;
            playerInside = false;
        }
    }

    public bool HasEnteredOnce()
    {
        return enteredOnce;
    }
}
