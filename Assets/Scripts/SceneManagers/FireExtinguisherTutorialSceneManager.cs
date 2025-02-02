using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExtinguisherTutorialSceneManager : MonoBehaviour {
    private TaskManager taskManager;
    [SerializeField] private SimpleFire fire;
    private bool ignitedOnce = false;

    private void Awake() {
        taskManager = GetComponent<TaskManager>();
    }

    public void PickUp() {
        taskManager.CompleteTask("PickupFireExtinguisher");
    }

    public void SafetyPin()
    {
        if (!taskManager.IsTaskCompleted("PickupFireExtinguisher"))
        {
            taskManager.GenerateTaskMistake("Tartu ensin kiinni sammuttimesta toisella k�dell�", 0);
                return;
        }
        taskManager.CompleteTask("SafetyPin");

    }

    public void Hose()
    {
        if (!taskManager.IsTaskCompleted("SafetyPin"))
        {
            taskManager.GenerateTaskMistake("Irroita ensin sokka", 0);
            return;
        }
        taskManager.CompleteTask("Hose");
    }

    public void Activate()
    {
        if (!taskManager.IsTaskCompleted("Hose"))
        {
            taskManager.GenerateTaskMistake("Tartu ensin kiinni sammuttimen letkun p��st�", 0);
            return;
        }
        taskManager.CompleteTask("ActivateFireExtinguisher");

        if (!ignitedOnce) {
            fire.Ignite();
            ignitedOnce = true;
        }
    }


    public void Extinguish()
    {
        taskManager.CompleteTask("Extinguish");
    }
}
