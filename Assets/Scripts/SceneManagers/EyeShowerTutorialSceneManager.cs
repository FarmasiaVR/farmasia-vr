using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeShowerTutorialSceneManager : MonoBehaviour
{
    private TaskManager taskManager;
    public GameObject AcidVisuals;
    // Start is called before the first frame update
    void Start() {
        taskManager = GetComponent<TaskManager>();
        //TODO: Add acid visuals back
        //AcidVisuals.SetActive(false);
    }

    public void PickUp() {
        taskManager.CompleteTask("PickupEyeShower");
    }

    public void Activate() {
        taskManager.CompleteTask("ActivateEyeShower");
        //AcidVisuals.SetActive(true);
    }

    public void Aim() {
        if (taskManager.IsTaskCompleted("ActivateEyeShower")) {
            taskManager.CompleteTask("AimEyeShower");
            //AcidVisuals.SetActive(false);
        }
    }
}

