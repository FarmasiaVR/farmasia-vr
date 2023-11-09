using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyShowerTutorialSceneManager : MonoBehaviour
{
    private TaskManager taskManager;
    private TaskboardManager taskboardManager;
    private bool playerExtinguished = false;
    
    private void Awake() {
        taskManager = GetComponent<TaskManager>();
        taskboardManager = GetComponent<TaskboardManager>();
    }

    public void SetPlayerExtinguished() {
        if(!playerExtinguished) {
            playerExtinguished = true;
            taskManager.CompleteTask("PlayerOnFire");
        }
    }

    public void PlayerExtinguishFailed() {
        //TODO: Ask old team about refactoring TaskManager. Scene specific scene manager scripts
        //like this one should contain all the logic for adding the tasks. TaskList as an object
        //inside Unity editor just makes things more unclear and split across many places.
        taskManager.onTaskFailed.Invoke(taskManager.GetTask("PlayerOnFire"));
    }
}