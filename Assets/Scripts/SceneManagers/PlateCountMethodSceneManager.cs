using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCountMethodSceneManager : MonoBehaviour
{
    private TaskManager taskManager;
    private TaskboardManager taskboardManager;

    private void Awake()
    {
        taskManager = GetComponent<TaskManager>();
        taskboardManager = GetComponent<TaskboardManager>();
    }
    public void CompleteTask(string taskName)
    {
        taskManager.CompleteTask(taskName);
        taskboardManager.MarkTaskAsCompleted(taskName);
    }
    public void CleanHands()
    {
        CompleteTask("WashHands");
    }
}
