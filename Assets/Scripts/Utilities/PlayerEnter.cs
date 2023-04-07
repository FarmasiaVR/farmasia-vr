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
        startingMaterial = gameObject.GetComponent<MeshRenderer>().material;
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        playerEntered = false;
        playerInside = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerEntered)
        {
            meshRenderer.material = enteredMaterial;
            greenShownTime -= Time.deltaTime;
            if(greenShownTime < 0)
            {
                meshRenderer.material = startingMaterial;
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
            MeshRenderer r = gameObject.GetComponent<MeshRenderer>();
            r.material = enteredMaterial;
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
