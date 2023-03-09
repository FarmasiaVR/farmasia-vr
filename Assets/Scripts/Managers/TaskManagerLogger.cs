using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FarmasiaVR.New;

public class TaskManagerLogger : MonoBehaviour
{
    public void PrintTaskStarted(Task task)
    {
        Debug.Log("Started task " + task.key + ". " + task.taskText);
        Debug.Log("Hint: " + task.hint);
    }

    public void PrintTaskCompleted(Task task)
    {
        Debug.Log("Completed task " + task.key + ": " + task.taskText);
        if (task.timed)
        {
            Debug.Log("Task " + task.key + " completed in " + task.timeTakenToCompleteTask);
        }
    }
}
