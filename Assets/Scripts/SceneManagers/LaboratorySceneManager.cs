using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaboratorySceneManager : MonoBehaviour
{

    private TaskManager taskManager;
    private TaskboardManager taskboardManager;
    private PlayerEnter[] playerEnter;
    private bool fireBlanketFound, eyeShowerFound, extinguisherFound, emergencyShowerFound;

    private void Awake() {
        taskManager = GetComponent<TaskManager>();
        taskboardManager = GetComponent<TaskboardManager>();
        playerEnter = FindObjectsOfType<PlayerEnter>();
    }

    public void CompleteTask(string taskName) {
        taskManager.CompleteTask(taskName);
        taskboardManager.MarkTaskAsCompleted(taskName);
        //TODO: Move TaskList into TaskManager and maybe add the option to directly read the 
        // tasklist hash table (shouldn't be able to write)
    }
}
