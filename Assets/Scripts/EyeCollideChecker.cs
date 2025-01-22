using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeCollideChecker : MonoBehaviour
{

    public EyeShowerTutorialSceneManager tutorialManager;
    public bool isRealisticScenario = false;
    public EyeShowerScenarioManager scenarioManager;
    private bool inside;

    void Start()
    {
        inside = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerCollider")
        {
            inside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "PlayerCollider")
        {
            inside = false;
        }
    }

    public void Activate()
    {
        if (inside)
        {
            if (isRealisticScenario)
            {
                scenarioManager.Aim();
            }
            else
            {
                tutorialManager.Aim();
            }
        }
    }
}
