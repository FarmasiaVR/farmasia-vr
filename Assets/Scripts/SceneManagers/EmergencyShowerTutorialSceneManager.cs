using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyShowerTutorialSceneManager : MonoBehaviour
{
    private TaskManager taskManager;
    private TaskboardManager taskboardManager;
    private bool playerExtinguished = false;
    
    private void Start() {
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
        taskManager.onTaskFailed.Invoke(taskManager.GetTask("PlayerOnFire"));
    }
}