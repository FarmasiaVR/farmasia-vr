using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExtinguisherTutorialSceneManager : MonoBehaviour {
    private TaskManager taskManager;
    [SerializeField] private SimpleFire fire;

    private void Awake() {
        taskManager = GetComponent<TaskManager>();
    }

    public void PickUp() {
        taskManager.CompleteTask("Pickup");
    }

    public void SafetyPin()
    {
        if (!taskManager.IsTaskCompleted("Pickup"))
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
        taskManager.CompleteTask("Activate");

        fire.Ignite();
    }


    public void Extinguish()
    {
        taskManager.CompleteTask("Extinguish");
    }
}
