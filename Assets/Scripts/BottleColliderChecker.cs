using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionTracker : MonoBehaviour
{

    private bool bottleInGrab = false;
    public EyeShowerScenarioManager sceneManager;

    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "BurstArea" & bottleInGrab)
        {
            sceneManager.BurstLiquid();
        }
    }

    public void BottleGrabbed()
    {
        bottleInGrab = true;
    }

    public void BottleReleased()
    {
        bottleInGrab = false;
    }

}
