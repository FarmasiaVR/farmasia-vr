using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCountMethodSceneManager : MonoBehaviour
{
    private TaskManager taskManager;

    private void Awake()
    {
        taskManager = GetComponent<TaskManager>();
    }
    public void CompleteTask(string taskName)
    {
        Debug.Log($"Trying to complete task");
        taskManager.CompleteTask(taskName);
    }
    public void CleanHands()
    {
        CompleteTask("WashHands");
    }
    public void GeneralMistake(string message, int penalty)
    {
        taskManager.GenerateGeneralMistake(message, penalty);
    }
}
